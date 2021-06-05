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

        public BackscratchersController(IBackScratcherRepository backScratcherRepository)
        {
            _backScratcherRepository = backScratcherRepository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateBackScratcherAsync([FromBody] BackScratcherRest body)
        {
            if (string.IsNullOrWhiteSpace(body.Name) ||
                string.IsNullOrWhiteSpace(body.Description) ||
                !body.Size.Any() ||
                string.IsNullOrWhiteSpace(body.PriceString))
            {
                return BadRequest();
            }

            BackScratcherDal dalModel = ConvertToDalModel(body);

            if ((await _backScratcherRepository.SearchBackScratcherAsync(dalModel.Name, dalModel.Description, dalModel.Size, dalModel.Price)).Any())
            {
                return Conflict();
            }

            Guid createdObjectId = await _backScratcherRepository.CreateBackScratcherAsync(dalModel.Name, dalModel.Description, dalModel.Size, dalModel.Price);

            return Created(HttpContext.Request.Path.Value, createdObjectId);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BackScratcherRest>>> SearchBackScratchersAsync(string name, string description, IEnumerable<string> size, string priceString)
        {

        }

        [HttpGet("{id}")] //TODO: figure out primary key
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BackScratcherRest>> GetBackScratcherAsync(Guid id)
        {

        }

        [HttpPut("{id}")] //TODO: figure out primary key
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateBackScratcherAsync(Guid id, [FromBody] BackScratcherRest body)
        {

        }

        [HttpDelete("{id}")] //TODO: figure out primary key
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> DeleteBackScratcherAsync(Guid id)
        {

        }

        private BackScratcherDal ConvertToDalModel(BackScratcherRest body)
        {
            throw new NotImplementedException();
        }
    }
}
