namespace RestaurantScheduler.Data.Interfaces
{
    public interface IDataSeeder
    {
        Task SeedFakeRestaurantsAsync();
        Task SeedFakeUsersAsync();
    }
}