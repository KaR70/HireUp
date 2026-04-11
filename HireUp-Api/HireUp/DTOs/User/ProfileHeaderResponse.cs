namespace HireUp.DTOs.User;

public class ProfileHeaderResponse
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string JobRole { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool IsVerified { get; set; }
}