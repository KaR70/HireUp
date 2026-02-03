using HireUp.Database.Interfaces;
using HireUp.DTOs.JobListing;

namespace HireUp.Services;

public class JobListingService : IJobListingService
{
    private readonly IJobListingRepository _jobListingRepository;

    public JobListingService(IJobListingRepository jobListingRepository)
    {
        _jobListingRepository = jobListingRepository;
    }

    public async Task<Result<IEnumerable<JobListingSummaryResponse>>> GetFeaturedAsync()
    {
        var featuredJobListings = await _jobListingRepository.GetFeaturedAsync();
        
        var response = featuredJobListings.Adapt<IEnumerable<JobListingSummaryResponse>>();

        return Result.Success(response);
    }

    public async Task<Result<IEnumerable<JobListingSummaryResponse>>> GetPopularAsync()
    {
        var popularJobListings = await _jobListingRepository.GetPopularAsync();
        
        var response = popularJobListings.Adapt<IEnumerable<JobListingSummaryResponse>>();

        return Result.Success(response);
    }

    public async Task<Result<JobListingDetailResponse>> GetByIdAsync(int id)
    {
        var jobListing = await _jobListingRepository.GetByIdAsync(id);
        
        if (jobListing is null)
            return Result.Failure<JobListingDetailResponse>(JobListingErrors.NotFound);

        var response = jobListing.Adapt<JobListingDetailResponse>();
        
        return Result.Success(response);
    }
}