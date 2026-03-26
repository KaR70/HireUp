namespace HireUp.DTOs.User;

public class PublicProfileResponse
{
    public string UserId { get; set; }
    public string FullName { get; set; }
    public string? Header { get; set; }
    public string? AboutMe { get; set; }
    public int ProjectCount { get; set; }
    public int FollowersCount { get; set; }
    public decimal Rating { get; set; }
    public string? ProfilePicture { get; set; }
    public List<SkillResponse> Skills { get; set; } = new();
}
