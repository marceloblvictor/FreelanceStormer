using Bogus;
using Microsoft.EntityFrameworkCore;
using RestaurantScheduler.Data.Interfaces;
using RestaurantScheduler.Models;

namespace RestaurantScheduler.Data
{
    public class DataSeeder : IDataSeeder
    {
        private readonly RestaurantSchedulerDbContext _dbContext;
        private readonly Faker<Organization> _userFaker;
        private readonly Faker<Restaurant> _restaurantFaker;

        public DataSeeder(
            RestaurantSchedulerDbContext dbContext,
            Faker<Organization> userFaker,
            Faker<Restaurant> restaurantFaker)
        {
            _dbContext = dbContext;
            _userFaker = userFaker;
            _restaurantFaker = restaurantFaker;
        }

        public async Task SeedFakeUsersAsync()
        {
            var users = new List<Organization>();

            _userFaker
                .RuleFor(u => u.Name,
                         f => f.Company.CompanyName())
                .RuleFor(u => u.PhoneNumber,
                         f => f.Phone.PhoneNumber())
                .RuleFor(u => u.TaxId,
                         f => f.Phone.PhoneNumber())
                .RuleFor(u => u.CreatedDate,
                         f => f.Date.Between(new DateTime(2022, 1, 1), new DateTime(2022, 12, 31)));

            int i = 0;
            while (i < 1000)
            {
                users.Add(_userFaker.Generate());
                i++;
            }
            _dbContext.Organizations.AddRange(users);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SeedFakeRestaurantsAsync()
        {
            var userIds = await _dbContext.Organizations
                .AsNoTracking()
                .Select(u => u.Id)
                .Distinct()
                .ToListAsync();

            var restaurants = new List<Restaurant>();

            _restaurantFaker
                .RuleFor(r => r.Name,
                         f => f.Company.CompanyName())
                .RuleFor(r => r.Seats,
                         f => f.Random.Int(16, 256))
                .RuleFor(r => r.Address,
                         f => f.Address.FullAddress())
                .RuleFor(u => u.CreatedDate,
                         f => f.Date.Between(new DateTime(2022, 1, 1), new DateTime(2022, 12, 31)))
                .RuleFor(r => r.UserId,
                         f => f.PickRandom(userIds));

            int i = 0;
            while (i < 1000)
            {
                restaurants.Add(_restaurantFaker.Generate());
                i++;
            }
            _dbContext.Restaurants.AddRange(restaurants);
            await _dbContext.SaveChangesAsync();
        }

    }
}
