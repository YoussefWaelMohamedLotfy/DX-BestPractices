namespace Learning.Domain.Entities;

public class Perk
{
    public int ID { get; set; }

    public string? Name { get; set; }

    public float Points { get; set; }

    public int PremiumUserID { get; set; }

    public PremiumUser? PremiumUser { get; set; }
}