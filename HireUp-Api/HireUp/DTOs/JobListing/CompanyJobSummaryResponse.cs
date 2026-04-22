namespace HireUp.DTOs.JobListing;

public record CompanyJobSummaryResponse(
    int Id,
    string Title,
    DateTime PostedAt,
    bool IsActive,
    int ApplicantsCount
);