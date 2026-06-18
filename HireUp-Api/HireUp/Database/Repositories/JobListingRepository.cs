using HireUp.Abstraction.Extention;
using Microsoft.EntityFrameworkCore;
using HireUp.Database.Interfaces;
using HireUp.DTOs;

namespace HireUp.Database.Repositories
{
    public class JobListingRepository : BaseRepository<JobListing>, IJobListingRepository
    {
        public JobListingRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<JobListing>> GetActiveListingsAsync()
            => await _dbSet.Where(j => j.IsActive && j.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow) && !j.IsDeleted)
                          .Include(j => j.Employer)
                          .Include(j => j.RequiredSkills)
                          .ToListAsync();

        public async Task<IEnumerable<JobListing>> GetInclusiveHiringListingsAsync()
            => await _dbSet.Where(j => j.IsInclusiveHiring && j.IsActive && j.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow) && !j.IsDeleted)
                          .Include(j => j.Employer)
                          .Include(j => j.RequiredSkills)
                          .ToListAsync();

        public async Task<IEnumerable<JobListing>> GetListingsBySkillsAsync(IEnumerable<int> skillIds)
            => await _dbSet.Where(j => j.IsActive && j.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow) && !j.IsDeleted && j.RequiredSkills.Any(s => skillIds.Contains(s.Id)))
                          .Include(j => j.Employer)
                          .Include(j => j.RequiredSkills)
                          .ToListAsync();

        public async Task<IEnumerable<JobListing>> SearchListingsAsync(string searchTerm, string location)
        {
            var query = _dbSet.Where(j => j.IsActive && j.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow) && !j.IsDeleted);

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(j => j.Title.Contains(searchTerm) || j.Description.Contains(searchTerm));
            
            //TODO: Modify That to work with the location entity
            // if (!string.IsNullOrEmpty(location) && location != "Any")
            //     query = query.Where(j => j.Location.Contains(location));

            return await query.Include(j => j.Employer).Include(j => j.RequiredSkills).ToListAsync();
        }

        public async Task<IEnumerable<JobListing>> GetListingsByEmployerAsync(string employerId)
            => await _dbSet.Where(j => j.EmployerId == employerId && !j.IsDeleted)
                          .Include(j => j.RequiredSkills)
                          .Include(j => j.Applications)
                          .ToListAsync();

        public async Task<IEnumerable<JobListing>> GetFeaturedAsync()
        {
            return await _dbSet
                .Where(j => j.IsFeatured && j.IsActive && !j.IsDeleted)
                .Include(j => j.Company)
                .Include(j => j.JobType)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.JobCategory)
                .Include(j => j.JobAccessibilityNeeds)
                    .ThenInclude(jan => jan.AccessibilityNeed)
                .ToListAsync();
        }

        public async Task<IEnumerable<JobListing>> GetPopularAsync()
        {
            return await _dbSet
                .Where(j => j.IsActive && j.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow) && !j.IsDeleted)
                .OrderByDescending(j => j.ViewCount)
                .Take(10)
                .Include(j => j.Company)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.JobCategory)
                .ToListAsync();
        }
        
        public async Task<JobListing?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(j => j.Company)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.JobType)
                .Include(j => j.JobCategory)
                .Include(j => j.JobAccessibilityNeeds)
                    .ThenInclude(jan => jan.AccessibilityNeed)
                .FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted);
        }

        public async Task<IEnumerable<JobListing>> GetByCompanyIdAsync(int companyId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId && !x.IsDeleted && x.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow))
                .Include(x => x.Applications)
                .ToListAsync(cancellationToken);
        
          
        public async Task<IEnumerable<JobListing>> SearchJobsAsync(JobSearchFilterDto filters, CancellationToken ct)
        {
            return await _dbSet
                .Include(j => j.Company)
                .Include(j => j.ExperienceLevel)
                .Include(j => j.JobType)
                .Include(j => j.Location)
                .Include(j => j.JobAccessibilityNeeds)
                    .ThenInclude(jan => jan.AccessibilityNeed)
                .AsQueryable()
                .ApplyFilters(filters)
                .ToListAsync(ct);
        }
    }
  
}