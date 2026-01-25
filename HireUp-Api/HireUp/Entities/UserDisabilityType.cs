namespace HireUp.Entities;

public class UserDisabilityType
{
    public string UserId { get; set; }
    public int DisabilityTypeId { get; set; }

    public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    public ApplicationUser User { get; set; } = null!;
    public DisabilityType DisabilityType { get; set; } = null!;
}
