using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using svc_backscratcher.DatabaseAccess;
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
        private readonly IDatabaseAccess _databaseAccess;

        public HealthChecksController(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetHealthCheckResult()
        {
            var connection = _databaseAccess.GetDatabaseConnection();
            return Ok($"Database connection: {connection.ConnectionString}");
        }
    }
}
