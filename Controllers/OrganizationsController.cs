using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantScheduler.Data.Interfaces;
using RestaurantScheduler.Models;
using Serilog;

namespace RestaurantScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IRestaurantSchedulerDbContext _dbContext;
        private readonly IDirectDbConnection _directConnection;

        public OrganizationsController(
            IRestaurantSchedulerDbContext dbContext,
            IDirectDbConnection directConnection)
        {
            _dbContext = dbContext;
            _directConnection = directConnection;
        }

        [HttpGet("{id}")]
        public async Task<Organization> GetByIdEF(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var data = await _dbContext.Organizations.FindAsync(id)
                ?? throw new Exception("Organization not found");

            stopWatch.Stop();

            Log.Warning($"{nameof(GetByIdEF)}: {stopWatch.Elapsed}s for Db retrieval");

            return data;
        }

        [HttpGet()]
        public async Task<IList<Organization>> GetListEF()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var data = await _dbContext.Organizations.ToListAsync();

            stopWatch.Stop();

            Log.Warning($"{nameof(GetListEF)}: {stopWatch.Elapsed}s for Db retrieval");

            return data;
        }

        [HttpGet("direct/{id}")]
        public async Task<Organization> GetByIdDirectly(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var data = await _directConnection.QuerySingleOrDefaultAsync<Organization>(
                "SELECT * FROM Organizations WHERE Id = @Id",
                new { Id = id })
                    ?? throw new Exception("Organization not found.");

            stopWatch.Stop();

            Log.Warning($"{nameof(GetByIdDirectly)}: {stopWatch.Elapsed}s for Db retrieval");

            return data;
        }

        [HttpGet("direct/")]
        public async Task<IReadOnlyList<Organization>> GetListDirectly()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var data = await _directConnection.QueryAsync<Organization>("SELECT * FROM Organizations");

            stopWatch.Stop();

            Log.Warning($"{nameof(GetListDirectly)}: {stopWatch.Elapsed}s for Db retrieval");

            return data;
        }
    }
}
