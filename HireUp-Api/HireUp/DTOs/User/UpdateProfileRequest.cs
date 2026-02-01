using HireUp.DTOs.DisabilityType;

namespace HireUp.DTOs.User;

public class UpdateProfileRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Bio { get; set; }
    public List<int> DisabilityTypeIds { get; set; } = new();
    public List<int> AccessibilityNeedsIds { get; set; } = new();
}