namespace HireUp.Entities;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string AuthorId { get; set; }
    public string ReviewedUserId { get; set; }
    public int JobApplicationId { get; set; }

    public ApplicationUser Author { get; set; }
    public ApplicationUser ReviewedUser { get; set; }
    public JobApplication JobApplication { get; set; }
}