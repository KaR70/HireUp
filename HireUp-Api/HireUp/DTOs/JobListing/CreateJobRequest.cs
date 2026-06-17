namespace HireUp.DTOs.JobListing;

public record CreateJobRequest(
    string Title,
    string Description,
    string Requirements,
    DateOnly EndDate,
    decimal Salary,
    int JobTypeId,
    int ExperienceLevelId,
    int LocationId,
    List<int> AccessibilityNeedIds
);