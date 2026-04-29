using HireUp.Dtos.User;
using HireUp.Entities;
using HireUp.Interfaces;

namespace HireUp.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _repository;
        private readonly UrlBuilderService _urlBuilderService;

        public JobApplicationService(IJobApplicationRepository repository, UrlBuilderService urlBuilderService)
        {
            _repository = repository;
            _urlBuilderService = urlBuilderService;
        }

        public async Task<IEnumerable<JobApplicationSummaryResponse>> GetMyApplicationsAsync(string userId)
        {
            var applications = await _repository.GetUserApplicationsAsync(userId);

            return applications.Select(app => new JobApplicationSummaryResponse
            {
                Id = app.Id,
                JobTitle = app.JobListing.Title,
                CompanyName = app.JobListing.Company.Name,
                CompanyLogoUrl = _urlBuilderService.ToAbsoluteUrl(app.JobListing.Company.Logo),
                Status = app.Status.ToString(), 
                AppliedAt = app.AppliedAt
            });
        }
    }
}