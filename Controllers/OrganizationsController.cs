using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantScheduler.Data.Interfaces;
using RestaurantScheduler.Models;

namespace RestaurantScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IRestaurantSchedulerDbContext _dbContext;
        private readonly IDirectDbConnection _directConnection;
        private readonly Serilog.ILogger _logger;

        public OrganizationsController(
            IRestaurantSchedulerDbContext dbContext,
            IDirectDbConnection directConnection,
            Serilog.ILogger logger)
        {
            _dbContext = dbContext;
            _directConnection = directConnection;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<Organization> GetByIdEF(int id)
        {
            return await _dbContext.Organizations.FindAsync(id)
                ?? throw new Exception("Organization not found");            
        }

        [HttpGet()]
        public async Task<IList<Organization>> GetListEF()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var data = await _dbContext.Organizations.ToListAsync();

            stopWatch.Stop();

            _logger.Warning($"{nameof(GetListEF)}: {stopWatch.Elapsed}\n");

            return data;
        }

        [HttpGet("direct/{id}")]
        public async Task<Organization> GetByIdDirectly(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var data = await _dbContext.Organizations.FindAsync(id)
                ?? throw new Exception("Organization not found");

            stopWatch.Stop();

            _logger.Warning($"{nameof(GetByIdDirectly)}: {stopWatch.Elapsed}\n");

            return data;
        }

        [HttpGet("direct/")]
        public async Task<IList<Organization>> GetListDirectly()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var data = await _directConnection.QueryAsync();

            stopWatch.Stop();

            _logger.Warning($"{nameof(GetListDirectly)}: {stopWatch.Elapsed}\n");

            return data;
        }
    }
}
