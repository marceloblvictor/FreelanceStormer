using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using FreelanceStormer.Models;

namespace FreelanceStormer.Data.Interfaces
{
    public interface IFreelanceStormerDbContext
    {
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
        public DbSet<Organization> Organizations { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}