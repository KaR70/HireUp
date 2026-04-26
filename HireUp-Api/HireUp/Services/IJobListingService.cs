using HireUp.DTOs.JobListing;

namespace HireUp.Services;

public interface IJobListingService
{
    Task<Result<IEnumerable<JobListingSummaryResponse>>> GetFeaturedAsync();
    Task<Result<IEnumerable<JobListingSummaryResponse>>> GetPopularAsync();
    Task<Result<JobListingDetailResponse>> GetByIdAsync(int id);

    Task<Result<IEnumerable<CompanyJobSummaryResponse>>> GetCompanyJobSummariesAsync(string userId, CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(string userId, int jobId, UpdateRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string userId, int jobId, CancellationToken cancellationToken = default);
}