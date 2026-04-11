using System.Linq.Expressions;
using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories;

public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<decimal?> GetAverageRatingForUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(r => r.ReviewedUserId == userId)
            .AverageAsync(r => (decimal?)r.Rating, cancellationToken);
    }
}