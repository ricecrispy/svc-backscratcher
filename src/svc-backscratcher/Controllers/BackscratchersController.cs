﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public BackscratchersController(IBackScratcherRepository backScratcherRepository, IMapper mapper)
        {
            _backScratcherRepository = backScratcherRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateBackScratcherAsync([FromBody] BackScratcherRest body)
        {
            if (string.IsNullOrWhiteSpace(body.Name) ||
                string.IsNullOrWhiteSpace(body.Description) ||
                !AreSizesValid(body.Sizes) ||
                !IsPriceValid(body.Price))
            {
                return BadRequest();
            }

            BackScratcherDal dalModel = _mapper.Map<BackScratcherDal>(body);
            var backScratchers = await _backScratcherRepository.SearchAllBackScraterchersAsync();
            if (backScratchers.Any(x => x.Name == dalModel.Name && x.Description == dalModel.Description && x.Size == dalModel.Size && x.Price == dalModel.Price))
            {
                return Conflict();
            }

            Guid createdObjectId = await _backScratcherRepository.CreateBackScratcherAsync(dalModel);

            return Created(HttpContext.Request.Path.Value, createdObjectId);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BackScratcherRest>>> SearchBackScratchersAsync()
        {
            return _mapper.Map<List<BackScratcherRest>>(await _backScratcherRepository.SearchAllBackScraterchersAsync());
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

            BackScratcherDal response = await _backScratcherRepository.GetBackScratcherAsync(id);
            if (response == null)
            {
                return NotFound();
            }

            return _mapper.Map<BackScratcherRest>(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateBackScratcherAsync(Guid id, [FromBody] BackScratcherRest body)
        {
            if ((id != default && id != body.Identifier) ||
                string.IsNullOrWhiteSpace(body.Name) ||
                string.IsNullOrWhiteSpace(body.Description) ||
                !AreSizesValid(body.Sizes) ||
                !IsPriceValid(body.Price))
            {
                return BadRequest();
            }

            if (id == default)
            {
                return await CreateBackScratcherAsync(body);
            }
            else
            {
                if (await _backScratcherRepository.GetBackScratcherAsync(id) == null)
                {
                    return NotFound();
                }

                BackScratcherDal input = _mapper.Map<BackScratcherDal>(body);
                await _backScratcherRepository.UpdateBackScratcherAsync(input);
                return Ok();
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

            if (await _backScratcherRepository.GetBackScratcherAsync(id) == null)
            {
                return NotFound();
            }

            await _backScratcherRepository.DeleteBackScratcherAsync(id);
            return Ok();
        }

        private bool AreSizesValid(IEnumerable<string> sizes)
        {
            return sizes.All(x => Enum.TryParse<BackScratcherSize>(x, out _));
        }

        private bool IsPriceValid(string priceString)
        {
            return double.TryParse(priceString.Replace("$", ""), out _);
        }
    }
}
