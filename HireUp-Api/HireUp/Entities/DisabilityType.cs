namespace HireUp.Entities;

public class DisabilityType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public virtual ICollection<UserDisabilityType> UserDisabilityTypes { get; set; } = new List<UserDisabilityType>();
}
