using Learning.API.Extensions;
using Learning.API.Options;
using Learning.Infrastructure.Data;
using Learning.Infrastructure.Extensions;
using Learning.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

builder.Services.AddDbContextPool<AppDbContext>((serviceProvider, options) =>
{
    var dbOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

    options.UseSqlite(dbOptions.ConnectionString, sqliteOptions =>
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
