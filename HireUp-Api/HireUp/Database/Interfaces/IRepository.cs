using System.Linq.Expressions;

namespace HireUp.Database.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsNoTrackingAsync(CancellationToken cancellationToken);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(int id);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
    }
}