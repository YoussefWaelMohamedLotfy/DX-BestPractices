using Learning.API.Extensions;
using Learning.Infrastructure.Data;
using Learning.Infrastructure.Extensions;
using Learning.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextPool<AppDbContext>(options
    => options.UseSqlite(builder.Configuration.GetConnectionString("Default")))
    .AddScoped<CourseRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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
