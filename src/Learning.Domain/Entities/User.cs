namespace Learning.Domain.Entities;

public abstract class User
{
    public int ID { get; set; }

    public string? FullName { get; set; }

    public DateTimeOffset DateOfBirth { get; set; }

    public List<Course> CoursesEnrolled { get; set; } = default!;
}

public class NormalUser : User
{
    public int NumberOfLogins { get; set; }
}

public class PremiumUser : User
{
    public string? UserName { get; set; }

    public string? Bio { get; set; }
}
