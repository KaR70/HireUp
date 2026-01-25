namespace HireUp.Entities;

public class UserAccessibilityNeed
{
    public string UserId { get; set; }
    public int AccessibilityNeedId { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public AccessibilityNeed AccessibilityNeed { get; set; } = null!;
}
