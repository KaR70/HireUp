namespace HireUp.DTOs.Company;

public record RecentApplicantDto(
    int ApplicationId,
    string ApplicantId,
    string ApplicantName,
    string JobRole,
    DateTime AppliedAt,
    string? ApplicantProfilePictureUrl
);