using HireUp.DTOs.Location;

namespace HireUp.DTOs.User;

public class MyProfileResponse
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public JobRoleResponse? JobRole { get; set; }
    public LocationSummaryResponse? Location { get; set; }
    public string? ProfilePictureUrl { get; set; }
}
