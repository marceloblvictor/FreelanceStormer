using System.Data;
using Microsoft.EntityFrameworkCore;
using FreelanceStormer.Data.Interfaces;
using FreelanceStormer.Models;

namespace FreelanceStormer.Data
{
    public class FreelanceStormerDbContext : DbContext, IFreelanceStormerDbContext
    {
        public FreelanceStormerDbContext(DbContextOptions<FreelanceStormerDbContext> options)
            : base(options)
        {
        }

        public IDbConnection Connection => Database.GetDbConnection();

        public DbSet<Organization> Organizations { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.LogTo(Console.WriteLine); 

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
