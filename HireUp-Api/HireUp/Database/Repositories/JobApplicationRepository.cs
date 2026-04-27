using Microsoft.EntityFrameworkCore;
using HireUp.Entities;
using HireUp.Interfaces; 
using HireUp.Database;      

namespace HireUp.Repositories
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobApplication>> GetUserApplicationsAsync(string userId)
        {
            return await _context.JobApplications
                .Include(a => a.JobListing)
                    .ThenInclude(j => j.Company) 
                .Where(a => a.JobSeekerId == userId)
                //.OrderByDescending(a => a.AppliedAt) 
                .ToListAsync();
        }
    }
}