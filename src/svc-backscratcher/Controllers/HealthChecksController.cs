using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.Controllers
{
    [ApiController]
    [Route("/health")]
    public class HealthChecksController : ControllerBase
    {
        public HealthChecksController()
        {

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetHealthCheckResult()
        {
            return Ok();
        }
    }
}
