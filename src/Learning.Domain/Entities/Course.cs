namespace Learning.Domain.Entities;

public sealed class Course
{
    public int ID { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public List<User> UsersEnrolled { get; set; } = default!;

    public List<Module> Modules { get; set; } = default!;
}
