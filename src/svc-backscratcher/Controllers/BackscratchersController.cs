using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Npgsql;
using svc_backscratcher.DataAccessLayers;
using svc_backscratcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class BackscratchersController : ControllerBase
    {
        private readonly IBackScratcherRepository _backScratcherRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BackscratchersController> _logger;

        public BackscratchersController(IBackScratcherRepository backScratcherRepository, IMapper mapper, ILogger<BackscratchersController> logger)
        {
            _backScratcherRepository = backScratcherRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateBackScratcherAsync([FromBody] BackScratcherRest body)
        {
            if (string.IsNullOrWhiteSpace(body.Name) ||
                string.IsNullOrWhiteSpace(body.Description) ||
                body.Sizes == null ||
                !AreSizesValid(body.Sizes) ||
                string.IsNullOrWhiteSpace(body.Price) ||
                !Util.TryGetProductPrice(body.Price, out _))
            {
                return BadRequest();
            }

            try
            {
                BackScratcherDal dalModel = _mapper.Map<BackScratcherDal>(body);
                var backScratchers = await _backScratcherRepository.SearchAllBackScraterchersAsync();
                if (backScratchers.Any(x => x.Name == dalModel.Name && x.Description == dalModel.Description && Enumerable.SequenceEqual(x.Size, dalModel.Size) && x.Price == dalModel.Price))
                {
                    return Conflict();
                }

                Guid createdObjectId = await _backScratcherRepository.CreateBackScratcherAsync(dalModel);

                return Created(HttpContext.Request.Path.Value, createdObjectId);
            }
            catch (NpgsqlException e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BackScratcherRest>>> SearchBackScratchersAsync(string name = null, string description = null, string sizes = null, string price = null)
        {
            try
            {
                var backscratchers = await _backScratcherRepository.SearchAllBackScraterchersAsync();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    backscratchers = backscratchers.Where(x => x.Name == name);
                }
                if (!string.IsNullOrWhiteSpace(description))
                {
                    backscratchers = backscratchers.Where(x => x.Description.ToLower().Contains(description.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(sizes))
                {
                    var sizeList = sizes.Split(",").ToList();
                    if (AreSizesValid(sizeList))
                    {
                        backscratchers = backscratchers.Where(x => x.Size.Any(s => sizeList.Contains(s.ToString())));
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                double priceLimit = double.MaxValue;
                if (!string.IsNullOrWhiteSpace(price))
                {
                    if (Util.TryGetProductPrice(price, out double parsedPrice))
                    {
                        priceLimit = parsedPrice;
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                backscratchers = backscratchers.Where(x => x.Price <= priceLimit);
                return _mapper.Map<List<BackScratcherRest>>(backscratchers);
            }
            catch (NpgsqlException e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BackScratcherRest>> GetBackScratcherAsync(Guid id)
        {
            if (id == default)
            {
                return BadRequest();
            }

            try
            {
                BackScratcherDal response = await _backScratcherRepository.GetBackScratcherAsync(id);
                if (response == null)
                {
                    return NotFound();
                }

                return _mapper.Map<BackScratcherRest>(response);
            }
            catch (NpgsqlException e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateBackScratcherAsync(Guid id, [FromBody] BackScratcherRest body)
        {
            if (body.Identifier == default)
            {
                return await CreateBackScratcherAsync(body);
            }

            if (id == default ||
                body.Identifier != id ||
                string.IsNullOrWhiteSpace(body.Name) ||
                string.IsNullOrWhiteSpace(body.Description) ||
                body.Sizes == null ||
                !AreSizesValid(body.Sizes) ||
                string.IsNullOrWhiteSpace(body.Price) ||
                !Util.TryGetProductPrice(body.Price, out _))
            {
                return BadRequest();
            }

            try
            {
                if (await _backScratcherRepository.GetBackScratcherAsync(id) == null)
                {
                    return NotFound();
                }

                BackScratcherDal input = _mapper.Map<BackScratcherDal>(body);
                await _backScratcherRepository.UpdateBackScratcherAsync(input);
                return Ok();
            }
            catch (NpgsqlException e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBackScratcherAsync(Guid id)
        {
            if (id == default)
            {
                return BadRequest();
            }

            try
            {
                if (await _backScratcherRepository.GetBackScratcherAsync(id) == null)
                {
                    return NotFound();
                }

                await _backScratcherRepository.DeleteBackScratcherAsync(id);
                return Ok();
            }
            catch (NpgsqlException e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool AreSizesValid(IEnumerable<string> sizes)
        {
            return sizes != null && sizes.All(x => Util.TryConvertSize(x, out _));
        }
    }
}
