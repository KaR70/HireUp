using HireUp.DTOs.JobListing;

namespace HireUp.Services;

public interface IJobListingService
{
    Task<Result<IEnumerable<JobListingSummaryResponse>>> GetFeaturedAsync();
    Task<Result<IEnumerable<JobListingSummaryResponse>>> GetPopularAsync();
    Task<Result<JobListingDetailResponse>> GetByIdAsync(int id);

    Task<Result<IEnumerable<CompanyJobSummaryResponse>>> GetCompanyJobSummariesAsync(string userId,
        CancellationToken cancellationToken = default);
}