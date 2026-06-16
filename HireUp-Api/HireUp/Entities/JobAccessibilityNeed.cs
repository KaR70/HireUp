namespace HireUp.Entities;

public class JobAccessibilityNeed
{
    public int JobListingId { get; set; }
    public JobListing JobListing { get; set; } = null!;

    public int AccessibilityNeedId { get; set; }
    public AccessibilityNeed AccessibilityNeed { get; set; } = null!;
}