namespace HireUp.DTOs.JobListing;

public class JobListingSummaryResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string CompanyName { get; set; }
    public string CompanyLogoUrl { get; set; }
    public string SalaryDisplay { get; set; }
    public string Location { get; set; }
    public List<string> Tags { get; set; } = new();
}