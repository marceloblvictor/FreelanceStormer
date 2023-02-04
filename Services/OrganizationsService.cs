using FreelanceStormer.Data.Interfaces;
using FreelanceStormer.Models;
using FreelanceStormer.Services.Interfaces;
using FreelanceStormer.Utils.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FreelanceStormer.Services
{
    public class OrganizationsService : IOrganizationsService
    {
        private readonly IFreelanceStormerDbContext _dbContext;
        private readonly IDirectDbConnection _directConnection;
        private readonly IDataCache _cache;

        public OrganizationsService(
            IFreelanceStormerDbContext dbContext,
            IDirectDbConnection dbDirect,
            IDataCache cache)
        {
            _dbContext = dbContext;
            _directConnection = dbDirect;
            _cache = cache;
        }

        public async Task<IReadOnlyList<Organization>> GetAll(BasicQuery query)
        {
            return (await _dbContext.Organizations
                .AsNoTracking()
                .Where(o => !string.IsNullOrWhiteSpace(query.Search) ?
                                o.Name.Contains(query.Search)
                                : true)
                .OrderByDescending(o => o.Id)
                .Skip(query.Skip)
                .Take(query.Size)
                .ToListAsync())
                // with this line you make it impossible to modify the list even with it is casted to List<T>
                .AsReadOnly();
        }

        public async Task<Organization> Get(int id)
        {
            var org = _cache.Get<Organization>(id);

            if (org is null)
            {
                org = await _dbContext.Organizations
                    .AsNoTracking()
                    .Where(o => o.Id == id)
                    .SingleOrDefaultAsync()
                        ?? throw new Exception("Organization not found");

                _cache.Set(id, org);
            }

            return org;
        }

        public async Task<IReadOnlyList<Organization>> GetAllWithRawSql()
        {
            return await _directConnection.QueryAsync<Organization>(
                "SELECT * FROM Organizations");
        }

        public async Task<Organization> GetWithRawSql(int id)
        {
            var org = _cache.Get<Organization>(id);

            if (org is null)
            {
                org = await _directConnection.QuerySingleOrDefaultAsync<Organization>(
                "SELECT * FROM Organizations WHERE Id = @Id",
                new { Id = id })
                    ?? throw new Exception("Organization not found.");

                _cache.Set(id, org);
            }

            return org;
        }
    }
}
