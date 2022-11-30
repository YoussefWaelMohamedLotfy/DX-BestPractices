namespace Learning.Domain.Entities;

public class Module
{
    public int ID { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int Order { get; set; }

    public int CourseID { get; set; }

    public Course? Course { get; set; }
}
