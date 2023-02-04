using System.Diagnostics;
using FreelanceStormer.Models;
using FreelanceStormer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;

namespace FreelanceStormer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IOrganizationsService _organizationsService;

        public OrganizationsController(
            IOrganizationsService organizationsService)
        {
            _organizationsService = organizationsService;

            // Contextual loggers are useful for one-off, or local contextual information like the source type name.                        
            _logger = Log.ForContext("Source", nameof(OrganizationsController));
            // same as:
            //_logger = Log.ForContext<OrganizationsController>();

            // Add a property to the log context uniquely for the log being done in the block
            using (LogContext.PushProperty(
                        "ThreadId", 
                        Thread.CurrentThread.ManagedThreadId.ToString(), 
                        destructureObjects: false))
            {
                // Process request; all logged events will carry `RequestId`
                _logger.Warning("Initiating controller...");
            }
        }

        [HttpGet("{id}")]
        public async Task<Organization> GetById(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var org = await _organizationsService.Get(id);

            stopWatch.Stop();

            _logger.Warning("{Endpoint} - DB Access Duration: {Elapsed}s. Entity retrieved: {@Organization}",
                nameof(GetById),
                stopWatch.Elapsed.ToString(),
                org);

            return org;
        }

        [HttpGet()]
        public async Task<IReadOnlyList<Organization>> GetList(
            [FromQuery] BasicQuery query)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var orgs = await _organizationsService.GetAll(query);

            stopWatch.Stop();

            _logger.Warning($"{nameof(GetList)}: {stopWatch.Elapsed}s. {orgs.Count} rows retrieved");

            return orgs;
        }

        [HttpGet("direct/{id}")]
        public async Task<Organization> GetByIdDirectly(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var data = await _organizationsService.GetWithRawSql(id);

            stopWatch.Stop();

            _logger.Warning($"{nameof(GetByIdDirectly)}: {stopWatch.Elapsed}");

            return data;
        }

        [HttpGet("direct/")]
        public async Task<IReadOnlyList<Organization>> GetListDirectly()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var orgs = await _organizationsService.GetAllWithRawSql();

            stopWatch.Stop();

            _logger.Warning($"{nameof(GetListDirectly)}: {stopWatch.Elapsed}s. {orgs.Count} rows retrieved");

            return orgs;
        }
    }
}
