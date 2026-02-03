using Microsoft.EntityFrameworkCore;
using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories
{
    public class JobListingRepository : BaseRepository<JobListing>, IJobListingRepository
    {
        public JobListingRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<JobListing>> GetActiveListingsAsync()
            => await _dbSet.Where(j => j.IsActive && j.ExpiryDate > DateTime.UtcNow)
                          .Include(j => j.Employer)
                          .Include(j => j.RequiredSkills)
                          .ToListAsync();

        public async Task<IEnumerable<JobListing>> GetInclusiveHiringListingsAsync()
            => await _dbSet.Where(j => j.IsInclusiveHiring && j.IsActive && j.ExpiryDate > DateTime.UtcNow)
                          .Include(j => j.Employer)
                          .Include(j => j.RequiredSkills)
                          .ToListAsync();

        public async Task<IEnumerable<JobListing>> GetListingsBySkillsAsync(IEnumerable<int> skillIds)
            => await _dbSet.Where(j => j.IsActive && j.ExpiryDate > DateTime.UtcNow && j.RequiredSkills.Any(s => skillIds.Contains(s.Id)))
                          .Include(j => j.Employer)
                          .Include(j => j.RequiredSkills)
                          .ToListAsync();

        public async Task<IEnumerable<JobListing>> SearchListingsAsync(string searchTerm, string location)
        {
            var query = _dbSet.Where(j => j.IsActive && j.ExpiryDate > DateTime.UtcNow);

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(j => j.Title.Contains(searchTerm) || j.Description.Contains(searchTerm));

            if (!string.IsNullOrEmpty(location) && location != "Any")
                query = query.Where(j => j.Location.Contains(location));

            return await query.Include(j => j.Employer).Include(j => j.RequiredSkills).ToListAsync();
        }

        public async Task<IEnumerable<JobListing>> GetListingsByEmployerAsync(string employerId)
            => await _dbSet.Where(j => j.EmployerId == employerId)
                          .Include(j => j.RequiredSkills)
                          .Include(j => j.Applications)
                          .ToListAsync();

        public async Task<IEnumerable<JobListing>> GetFeaturedAsync()
        {
            return await _dbSet
                .Where(j => j.IsFeatured && j.IsActive && j.ExpiryDate > DateTime.UtcNow)
                .Include(j => j.Company)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.JobCategory)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobListing>> GetPopularAsync()
        {
            return await _dbSet
                .Where(j => j.IsActive && j.ExpiryDate > DateTime.UtcNow)
                .OrderByDescending(j => j.ViewCount)
                .Take(10)
                .Include(j => j.Company)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.JobCategory)
                .ToListAsync();
        }
        
        public override async Task<JobListing?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(j => j.Company)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.JobCategory)
                .FirstOrDefaultAsync(j => j.Id == id);
        }
    }
}