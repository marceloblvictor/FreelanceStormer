using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RestaurantScheduler.Models;

namespace RestaurantScheduler.Data.Interfaces
{
    public interface IRestaurantSchedulerDbContext
    {
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}