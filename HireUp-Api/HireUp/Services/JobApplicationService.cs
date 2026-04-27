using HireUp.Dtos.User;
using HireUp.Entities;
using HireUp.Interfaces;

namespace HireUp.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _repository;

        public JobApplicationService(IJobApplicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<JobApplicationSummaryResponse>> GetMyApplicationsAsync(string userId)
        {
            var applications = await _repository.GetUserApplicationsAsync(userId);

            return applications.Select(app => new JobApplicationSummaryResponse
            {
                Id = app.Id,
                JobTitle = app.JobListing.Title,
                CompanyName = app.JobListing.Company.Name,
                CompanyLogoUrl = app.JobListing.Company.LogoUrl,
                Status = app.Status.ToString(), 
                AppliedAt = app.AppliedAt
            });
        }
    }
}