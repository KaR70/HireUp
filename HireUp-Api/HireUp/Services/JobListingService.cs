using HireUp.Database.Interfaces;
using HireUp.DTOs.JobListing;
using Microsoft.IdentityModel.Tokens;

namespace HireUp.Services;

public class JobListingService : IJobListingService
{
    private readonly IJobListingRepository _jobListingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public JobListingService(IJobListingRepository jobListingRepository, IUnitOfWork unitOfWork)
    {
        _jobListingRepository = jobListingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<JobListingSummaryResponse>>> GetFeaturedAsync()
    {
        var featuredJobListings = await _jobListingRepository.GetFeaturedAsync();
        
        // TODO: Fix here and the method below it to check if the result is empty before Mapping
        if (featuredJobListings.IsNullOrEmpty())
        {
            
        }
        
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
        var jobListing = await _jobListingRepository.GetByIdWithDetailsAsync(id);
        
        if (jobListing is null)
            return Result.Failure<JobListingDetailResponse>(JobListingErrors.NotFound);

        var response = jobListing.Adapt<JobListingDetailResponse>();
        
        return Result.Success(response);
    }

    public async Task<Result<IEnumerable<CompanyJobSummaryResponse>>> GetCompanyJobSummariesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByUserIdAsync(userId, cancellationToken);

        if (company is null)
            return Result.Failure<IEnumerable<CompanyJobSummaryResponse>>(CompanyErrors.NotFound);

        var jobListings = await _unitOfWork.JobListings.GetByCompanyIdAsync(company.Id, cancellationToken);
        
        var response = jobListings.Adapt<IEnumerable<CompanyJobSummaryResponse>>();
        
        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(string userId, int jobId, UpdateRequest request, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByUserIdAsync(userId, cancellationToken);

        if (company is null)
            return Result.Failure(CompanyErrors.NotFound);

        var job = await _unitOfWork.JobListings.GetByIdAsync(jobId);
        
        if(job is null)
            return Result.Failure(JobListingErrors.NotFound);
        
        if (job.CompanyId != company.Id)
            return Result.Failure(JobListingErrors.Forbidden);

        job.Title = request.Title;
        job.Description = request.Description;
        job.Requirements = request.Requirements;
        job.Salary = request.Salary;
        job.IsInclusiveHiring = request.IsInclusiveHiring;
        job.DisabilitySupport = request.DisabilitySupport;
        job.IsActive = request.IsActive;
        job.ExpiryDate = request.ExpiryDate;
        job.ExperienceLevelId = request.ExperienceLevelId;
        job.JobTypeId = request.JobRoleId;
        job.LocationId = request.LocationId;
        
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string userId, int jobId, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByUserIdAsync(userId, cancellationToken);

        if (company is null)
            return Result.Failure(CompanyErrors.NotFound);

        var job = await _unitOfWork.JobListings.GetByIdAsync(jobId);

        if (job is null)
            return Result.Failure(JobListingErrors.NotFound);

        if (job.CompanyId != company.Id)
            return Result.Failure(JobListingErrors.Forbidden);

        if (!job.IsDeleted)
        {
            job.IsDeleted = true;
            job.IsActive = false;
        }
        
        await _unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }
}