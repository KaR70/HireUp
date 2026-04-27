namespace HireUp.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Logo { get; set; }
    public string? Website { get; set; }
    public string? LinkedIn { get; set; }
    public int? FoundedYear { get; set; }

    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    
    public int? IndustryId { get; set; }
    public Industry? Industry { get; set; }
    
    public int? LocationId { get; set; }
    public Location? Location { get; set; }
    
    public virtual ICollection<JobListing> JobListings{ get; set; }
    public string LogoUrl { get; set; } = string.Empty;
}