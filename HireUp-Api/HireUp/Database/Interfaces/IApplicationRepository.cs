namespace HireUp.Database.Interfaces
{
    public interface IApplicationRepository : IRepository<JobApplication>
    {
        Task<IEnumerable<JobApplication>> GetApplicationsByJobSeekerAsync(string jobSeekerId);
        Task<IEnumerable<JobApplication>> GetApplicationsByJobListingAsync(int jobListingId);
        Task<JobApplication> GetApplicationDetailsAsync(int applicationId);
        Task<bool> HasAppliedAsync(string jobSeekerId, int jobListingId);
    }
}