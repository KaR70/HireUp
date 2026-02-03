namespace HireUp.Entities;

public class JobCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<JobListing> JobListings { get; set; } = new List<JobListing>();
}