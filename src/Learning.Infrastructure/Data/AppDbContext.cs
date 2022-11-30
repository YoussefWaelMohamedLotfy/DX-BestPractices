﻿using System.Reflection;

using Learning.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Learning.Infrastructure.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Perk> Perks { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Domain.Entities.Module> Modules { get; set; }
    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
