namespace HireUp.Entities;

public class AccessibilityNeed
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public virtual ICollection<UserAccessibilityNeed> UserAccessibilityNeeds { get; set; } = new List<UserAccessibilityNeed>();
    public virtual ICollection<JobAccessibilityNeed> JobAccessibilityNeeds { get; set; } = new List<JobAccessibilityNeed>();
}
