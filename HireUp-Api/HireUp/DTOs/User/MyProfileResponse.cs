using HireUp.DTOs.AccessibilityNeed;
using HireUp.DTOs.DisabilityType;

namespace HireUp.DTOs.User;

public class MyProfileResponse
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public List<DisabilityTypeResponse> DisabilityTypes { get; set; } = new();
    public List<AccessibilityNeedResponse> AccessibilityNeeds { get; set; } = new();
}
