using HireUp.Entities;

namespace HireUp.Database.Interfaces;

public interface ISavedJobRepository
{
    Task<IEnumerable<SavedJob>> GetAllAsync(string userId);
    Task AddAsync(SavedJob savedJob);
    Task RemoveAsync(int jobListingId, string userId);
    Task<bool> IsAlreadySavedAsync(int jobListingId, string userId);
}