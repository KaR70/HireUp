namespace HireUp.Database.Interfaces;

public interface IReviewRepository : IRepository<Review>
{
    Task<decimal?> GetAverageRatingForUserAsync(string userId, CancellationToken cancellationToken = default);
}