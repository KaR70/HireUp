namespace HireUp.DTOs.JobListing;

public class JobListingDetailResponse : JobListingSummaryResponse
{
    public string Description { get; set; }
    public string Requirements { get; set; }
    public string AboutCompany { get; set; }
    public List<string> AccessibilityNeeds { get; set; } = new();
}