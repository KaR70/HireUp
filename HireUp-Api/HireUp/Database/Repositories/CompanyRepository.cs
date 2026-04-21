using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories;

public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<int> CountActiveJobsAsync(string userId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(x => x.UserId == userId)
            .SelectMany(x => x.JobListings)
            .CountAsync(j => j.IsActive, cancellationToken);
    
    public async Task<int> CountTotalApplicantsAsync(string userId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(x => x.UserId == userId)
            .SelectMany(x => x.JobListings)
            .SelectMany(j => j.Applications)
            .CountAsync(cancellationToken);
    
    public async Task<Company> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
}