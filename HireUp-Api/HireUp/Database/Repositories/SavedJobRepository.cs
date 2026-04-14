using HireUp.Database.Interfaces;
using HireUp.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireUp.Database.Repositories;

public class SavedJobRepository : ISavedJobRepository
{
    private readonly ApplicationDbContext _context;

    public SavedJobRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SavedJob>> GetAllAsync(string userId)
    {
        return await _context.SavedJobs
            .Include(sj => sj.JobListing) // عشان لما نعرض المحفوظات نشوف بيانات الوظيفة
            .Where(sj => sj.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(SavedJob savedJob)
    {
        await _context.SavedJobs.AddAsync(savedJob);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int jobListingId, string userId)
    {
        var savedJob = await _context.SavedJobs
            .FirstOrDefaultAsync(sj => sj.JobListingId == jobListingId && sj.UserId == userId);

        if (savedJob != null)
        {
            _context.SavedJobs.Remove(savedJob);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsAlreadySavedAsync(int jobListingId, string userId)
    {
        return await _context.SavedJobs
            .AnyAsync(sj => sj.JobListingId == jobListingId && sj.UserId == userId);
    }
}