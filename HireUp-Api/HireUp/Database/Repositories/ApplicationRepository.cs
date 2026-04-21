using Microsoft.EntityFrameworkCore;
using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories
{
    public class ApplicationRepository : BaseRepository<JobApplication>, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<JobApplication>> GetApplicationsByJobSeekerAsync(string jobSeekerId)
            => await _dbSet.Where(a => a.JobSeekerId == jobSeekerId)
                          .Include(a => a.JobListing).ThenInclude(j => j.Employer)
                          .Include(a => a.JobListing).ThenInclude(j => j.RequiredSkills)
                          .OrderByDescending(a => a.AppliedAt)
                          .ToListAsync();

        public async Task<IEnumerable<JobApplication>> GetApplicationsByJobListingAsync(int jobListingId)
            => await _dbSet.Where(a => a.JobListingId == jobListingId)
                          .Include(a => a.JobSeeker).ThenInclude(u => u.Skills)
                          .OrderByDescending(a => a.AppliedAt)
                          .ToListAsync();

        public async Task<JobApplication> GetApplicationDetailsAsync(int applicationId)
            => await _dbSet.Include(a => a.JobListing).ThenInclude(j => j.Employer)
                          .Include(a => a.JobSeeker).ThenInclude(u => u.Skills)
                          .FirstOrDefaultAsync(a => a.Id == applicationId);

        public async Task<bool> HasAppliedAsync(string jobSeekerId, int jobListingId)
            => await _dbSet.AnyAsync(a => a.JobSeekerId == jobSeekerId && a.JobListingId == jobListingId);

        public async Task<IEnumerable<JobApplication>> GetRecentApplicantsAsync(string companyUserId, int count = 5,
            CancellationToken cancellationToken = default)
            => await _dbSet.Where(x => x.JobListing.Company.UserId == companyUserId)
                .Include(x => x.JobSeeker)
                .Include(x => x.JobListing)
                .OrderByDescending(x => x.AppliedAt)
                .Take(count)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
    }
}