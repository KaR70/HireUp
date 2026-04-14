namespace HireUp.Entities;

public class Follows
{
    public string FollowerId { get; set; }
    public string FollowingId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public ApplicationUser Follower { get; set; }
    public ApplicationUser Following { get; set; }
}