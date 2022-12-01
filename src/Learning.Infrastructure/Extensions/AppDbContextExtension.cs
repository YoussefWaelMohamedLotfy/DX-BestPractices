using Bogus;
using Learning.Domain.Entities;
using Learning.Infrastructure.Data;

using Microsoft.Extensions.Logging;

namespace Learning.Infrastructure.Extensions;

public static class AppDbContextExtension
{
    public static async Task SeedAsync(this AppDbContext context, ILogger<AppDbContext> logger)
    {
        int userIds = 1;

        if (!context.Users.Any())
        {
            var fakeNormalUsersSchema = new Faker<NormalUser>()
                .RuleFor(u => u.ID, _ => userIds++)
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.DateOfBirth, f => f.Person.DateOfBirth)
                .RuleFor(u => u.NumberOfLogins, f => f.Random.Int(1, 100));

            await context.Users.AddRangeAsync(fakeNormalUsersSchema.Generate(10));

            var fakePremiumUsersSchema = new Faker<PremiumUser>()
                .RuleFor(u => u.ID, _ => userIds++)
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.DateOfBirth, f => f.Person.DateOfBirth)
                .RuleFor(u => u.UserName, f => f.Person.UserName)
                .RuleFor(u => u.Bio, f => f.Lorem.Random.Words(10));

            await context.Users.AddRangeAsync(fakePremiumUsersSchema.Generate(10));
        }

        if (!context.Courses.Any())
        {
            int courseIds = 1;

            var fakeCoursesSchema = new Faker<Course>()
                .RuleFor(u => u.ID, _ => courseIds++)
                .RuleFor(u => u.Name, f => f.Database.Engine())
                .RuleFor(u => u.Description, f => f.Lorem.Random.Words(4));

            await context.Courses.AddRangeAsync(fakeCoursesSchema.Generate(5));
        }

        if (!context.Modules.Any())
        {
            int moduleIds = 1;
            int moduleOrder = 1;

            var fakeCoursesSchema = new Faker<Module>()
                .RuleFor(u => u.ID, _ => moduleIds++)
                .RuleFor(u => u.Name, f => f.Database.Engine())
                .RuleFor(u => u.Description, f => f.Lorem.Random.Words(4))
                .RuleFor(u => u.Order, _ => moduleOrder++);

            await context.Modules.AddRangeAsync(fakeCoursesSchema.Generate(10));
        }

        await context.SaveChangesAsync(CancellationToken.None);
        logger.LogInformation("Completed Seeding of {DbContextName}", typeof(AppDbContext).Name);
    }
}
