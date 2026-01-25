namespace HireUp.DTOs.User;

public class PublicProfileResponse
{
    public string UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? ProfilePicture { get; set; }
}
