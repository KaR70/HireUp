namespace HireUp.DTOs.Company;

public record CompanyHomeResponse(
    string Name,
    string? LogoUrl,
    int ActiveJobsCount,
    int TotalApplicantsCount,
    List<RecentApplicantDto> RecentApplicants
);