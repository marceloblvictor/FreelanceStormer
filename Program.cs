using FreelanceStormer;
using FreelanceStormer.Data;
using FreelanceStormer.Data.Interfaces;
using FreelanceStormer.Models;
using FreelanceStormer.Services;
using FreelanceStormer.Services.Interfaces;
using FreelanceStormer.Utils;
using FreelanceStormer.Utils.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        // when serializing the organization entity perform a transformation
        .Destructure.ByTransforming<Organization>(
            org => new { 
                            org.Name, 
                            org.PhoneNumber, 
                            org.Id, 
                            org.CreatedDate 
                        })
        .Enrich.FromLogContext());

Log.Information("Starting services configuration...");

// Add services to the container.
builder.Services.AddDbContext<FreelanceStormerDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("FreelanceStormerDbContext")));

builder.Services.AddScoped<IDataSeeder, DataSeeder>();
builder.Services.AddScoped<IFreelanceStormerDbContext>(
    provider => provider.GetRequiredService<FreelanceStormerDbContext>());
builder.Services.AddScoped<IDirectDbConnection, DirectDbConnection>();

builder.Services.AddScoped<IMemoryCache, MemoryCache>();
builder.Services.AddScoped<IDataCache, DataMemoryCache>();

builder.Services.AddScoped<IOrganizationsService, OrganizationsService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Information("Building application. Builder data: \n{@Builder}", 
    builder);

var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.GetLevel = (ctx, elapsed, ex) => LogEventLevel.Information;    
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

//    await seeder.SeedFakeOrganizationsAsync();
//}

app.Run();