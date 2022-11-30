namespace Learning.Domain.Entities;

public sealed class Subscription
{
    public int ID { get; set; }

    public string? Name { get; set; }

    public float Rank { get; set; }

    public int NormalUserID { get; set; }

    public NormalUser? NormalUser { get; set; }
}