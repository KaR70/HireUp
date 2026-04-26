namespace HireUp.DTOs.JobListing;

public record UpdateRequest(
    string Title,
    string Description,
    string Requirements,
    decimal Salary,
    bool IsInclusiveHiring,
    string? DisabilitySupport,
    DateOnly ExpiryDate,
    bool IsActive,
    int ExperienceLevelId,
    int JobRoleId,
    int LocationId
);