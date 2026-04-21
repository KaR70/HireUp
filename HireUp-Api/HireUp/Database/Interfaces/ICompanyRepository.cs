namespace HireUp.Database.Interfaces;

public interface ICompanyRepository : IRepository<Company>
{
    Task<int> CountActiveJobsAsync(string userId, CancellationToken cancellationToken = default);

    Task<int> CountTotalApplicantsAsync(string userId, CancellationToken cancellationToken = default);
    Task<Company> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}