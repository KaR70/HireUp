namespace HireUp.DTOs;

public class JobSearchFilterDto
{
    public string? SearchTerm { get; set; }
    public List<int>? AccessibilityNeedIds { get; set; }
    public int? ExperienceLevelId { get; set; }
    public int? JobTypeId { get; set; }
    public int? LocationId { get; set; }
    public int? OfficeTypeId { get; set; }
    public int? IndustryId { get; set; }
}