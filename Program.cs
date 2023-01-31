using Bogus;
using Microsoft.EntityFrameworkCore;
using RestaurantScheduler;
using RestaurantScheduler.Data;
using RestaurantScheduler.Data.Interfaces;
using RestaurantScheduler.Models;
using Serilog;
using Serilog.Events;

// Bootstrapped logger that will be replaced
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting builder...");

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog logger to output application general output
builder.Host.UseSerilog((context, services, configuration)
    => configuration
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.With<EventTypeEnricher>()
        .Enrich.FromLogContext());

Log.Information("Starting services configuration...");

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

Log.Information("Building application. Builder data: \n{@Builder}", 
    builder);

var app = builder.Build();
app.UseSerilogRequestLogging(options =>
{
    options.GetLevel = (ctx, elapsed, ex) => Serilog.Events.LogEventLevel.Warning;    
});

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