using Bogus;
using Microsoft.EntityFrameworkCore;
using RestaurantScheduler.Data;
using RestaurantScheduler.Data.Interfaces;
using RestaurantScheduler.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add and configure Serilog Logger
var logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.Console()
    .WriteTo.File($"logs/{DateTime.Now.Ticks}.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging
    .ClearProviders()
    .AddSerilog();

// Add services to the container.
builder.Services.AddDbContext<RestaurantSchedulerDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("RestaurantSchedulerDbContext")));

builder.Services.AddScoped<IDataSeeder, DataSeeder>();
builder.Services.AddScoped<IRestaurantSchedulerDbContext>(
    provider => provider.GetRequiredService<RestaurantSchedulerDbContext>());
builder.Services.AddScoped<IDirectDbConnection, DirectDbConnection>();

builder.Services.AddTransient<Faker<Restaurant>, Faker<Restaurant>>();
builder.Services.AddTransient<Faker<Organization>, Faker<Organization>>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Uncomment this to populate a newly created Db

//using (var serviceScope = app.Services.CreateScope())
//{
//    var seeder = serviceScope
//         .ServiceProvider
//         .GetRequiredService<IDataSeeder>();

//    await seeder.SeedFakeUsersAsync();
//    await seeder.SeedFakeRestaurantsAsync();
//}

app.Run();