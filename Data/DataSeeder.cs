using Bogus;
using FreelanceStormer.Data.Interfaces;
using FreelanceStormer.Models;

namespace FreelanceStormer.Data
{
    public class DataSeeder : IDataSeeder
    {
        private readonly FreelanceStormerDbContext _dbContext;

        public DataSeeder(
            FreelanceStormerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedFakeOrganizationsAsync()
        {
            var faker = new Faker<Organization>();

            var orgs = new List<Organization>();

            faker
                .RuleFor(u => u.Name,
                         f => f.Company.CompanyName())
                .RuleFor(u => u.PhoneNumber,
                         f => f.Phone.PhoneNumber())
                .RuleFor(u => u.TaxId,
                         f => f.Phone.PhoneNumber())
                .RuleFor(u => u.CreatedDate,
                         f => f.Date.Between(new DateTime(2022, 1, 1), new DateTime(2022, 12, 31)));

            int i = 0;
            while (i < 100000)
            {
                orgs.Add(faker.Generate());
                i++;
            }
            _dbContext.Organizations.AddRange(orgs);
            await _dbContext.SaveChangesAsync();
        }
    }
}
