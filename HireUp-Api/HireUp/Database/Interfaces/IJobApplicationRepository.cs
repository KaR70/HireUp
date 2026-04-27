using HireUp.Entities;

namespace HireUp.Interfaces 
{
    public interface IJobApplicationRepository
    {
        Task<IEnumerable<JobApplication>> GetUserApplicationsAsync(string userId);
    }
}