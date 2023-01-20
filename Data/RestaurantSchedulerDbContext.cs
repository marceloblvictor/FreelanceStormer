using System.Data;
using Microsoft.EntityFrameworkCore;
using RestaurantScheduler.Data.Interfaces;
using RestaurantScheduler.Models;

namespace RestaurantScheduler.Data
{
    public class RestaurantSchedulerDbContext : DbContext, IRestaurantSchedulerDbContext
    {
        public RestaurantSchedulerDbContext(DbContextOptions<RestaurantSchedulerDbContext> options)
            : base(options)
        {
        }

        public IDbConnection Connection => Database.GetDbConnection();

        public DbSet<Restaurant> Restaurants { get; set; } = default!;
        public DbSet<Organization> Organizations { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.LogTo(Console.WriteLine); 

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.Organization)
                    .WithMany(u => u.Restaurants);
        }
    }
}
