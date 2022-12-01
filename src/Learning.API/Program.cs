using Learning.Logging;
using Serilog;

using Learning.API.Extensions;
using Learning.API.Options;
using Learning.Infrastructure.Data;
using Learning.Infrastructure.Extensions;
using Learning.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Learning.API.FluentValidators;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.

// See https://andrewlock.net/adding-validation-to-strongly-typed-configuration-objects-using-flentvalidation/
builder.Services.AddWithValidation<DatabaseOptions, DatabaseOptionsValidator>("DatabaseOptions");

// Another way to configure DatabaseOptions, See https://www.youtube.com/watch?v=bN57EDYD6M0
//builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

builder.Services.AddDbContextPool<AppDbContext>((serviceProvider, options) =>
{
    var dbOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

    options.UseSqlite(builder.Configuration.GetConnectionString("Default"), sqliteOptions =>
    {
        sqliteOptions.CommandTimeout(dbOptions.CommandTimeout);
    });
    options.EnableDetailedErrors(dbOptions.EnableDetailedErrors);
    options.EnableSensitiveDataLogging(dbOptions.EnableSensitiveDataLogging);
})
    .AddScoped<CourseRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.MigrateDatabase<AppDbContext>(async (context, services) =>
    {
        var logger = services.GetRequiredService<ILogger<AppDbContext>>();
        await context.SeedAsync(logger);
    });

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
