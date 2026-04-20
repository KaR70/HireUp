using System.Linq.Expressions;
using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);  

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public virtual async Task<IEnumerable<T>> GetAllAsNoTrackingAsync(CancellationToken cancellationToken = default) 
            => await _dbSet.AsNoTracking().ToListAsync();

        public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public virtual Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public virtual async Task<bool> ExistsAsync(int id) => await _dbSet.FindAsync(id) != null;
        
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default) => 
            await _dbSet.FirstOrDefaultAsync(criteria, cancellationToken);
        
        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default)
            => await _dbSet.CountAsync(criteria, cancellationToken);
    
        public async Task<int> CountAsync(CancellationToken cancellationToken = default) => await _dbSet.CountAsync(cancellationToken);
    }
}