using AutoFixture;
using FreelanceStormer.Models;

namespace FreelanceStormer.Data
{
    public static class Seeder
    {
        public async static Task SeedAsync(this FreelanceStormerDbContext dbContext)
        {
            if (!dbContext.Organizations.Any())
            {
                Fixture fixture = new Fixture();
                fixture.Customize<Organization>(org => org.Without(o => o.Id));
                List<Organization> orgs = fixture.CreateMany<Organization>(1000).ToList();

                dbContext.AddRange(orgs);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
