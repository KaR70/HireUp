using HireUp.Dtos.User;

namespace HireUp.Interfaces
{
    public interface IJobApplicationService
    {
        Task<IEnumerable<JobApplicationSummaryResponse>> GetMyApplicationsAsync(string userId);
    }
}