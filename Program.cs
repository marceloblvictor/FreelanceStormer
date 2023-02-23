using FluentValidation;
using FreelanceStormer.Data;
using FreelanceStormer.Data.Interfaces;
using FreelanceStormer.Models;
using FreelanceStormer.Services;
using FreelanceStormer.Services.Interfaces;
using FreelanceStormer.Utils;
using FreelanceStormer.Utils.Interfaces;
using FreelanceStormer.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

// Options pattern with validation
builder.Services.AddOptions<DatabaseOptions>()
    .Bind(config.GetSection(DatabaseOptions.ConnectionStrings))
    .ValidateFluently()
    .ValidateOnStart();

// Bootstrapped logger that will be replaced
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting builder...");

// Configure Serilog logger to output application general output
builder.Host.UseSerilog((context, services, configuration)
    => configuration
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.With<EventTypeEnricher>()
        // when serializing the organization entity perform a transformation
        .Destructure.ByTransforming<Organization>(
            org => new
            {
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

//Seed data
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<FreelanceStormerDbContext>();
//    await dbContext.Database.MigrateAsync();
//    await dbContext.SeedAsync();
//}

app.UseSerilogRequestLogging(options =>
{
    options.GetLevel = (ctx, elapsed, ex) => LogEventLevel.Information;
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();