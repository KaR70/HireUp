namespace HireUp.Database.Interfaces
{
    public interface IJobListingRepository : IRepository<JobListing>
    {
        Task<IEnumerable<JobListing>> GetActiveListingsAsync();
        Task<IEnumerable<JobListing>> GetInclusiveHiringListingsAsync();
        Task<IEnumerable<JobListing>> GetListingsBySkillsAsync(IEnumerable<int> skillIds);
        Task<IEnumerable<JobListing>> SearchListingsAsync(string searchTerm, string location);
        Task<IEnumerable<JobListing>> GetListingsByEmployerAsync(string employerId);
        Task<IEnumerable<JobListing>> GetFeaturedAsync();
        Task<IEnumerable<JobListing>> GetPopularAsync();

        Task<IEnumerable<JobListing>> GetByCompanyIdAsync(int companyId,
            CancellationToken cancellationToken = default);
    }
}