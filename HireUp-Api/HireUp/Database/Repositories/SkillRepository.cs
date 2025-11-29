using Microsoft.EntityFrameworkCore;
using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories
{
    public class SkillRepository : BaseRepository<Skill>, ISkillRepository
    {
        public SkillRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Skill>> GetSkillsByCategoryAsync(SkillCategory category)
            => await _dbSet.Where(s => s.Category == category).ToListAsync();

        public async Task<IEnumerable<Skill>> SearchSkillsAsync(string searchTerm)
            => await _dbSet.Where(s => s.Name.Contains(searchTerm) || s.Description.Contains(searchTerm)).ToListAsync();

        public async Task<Skill> GetByNameAsync(string name)
            => await _dbSet.FirstOrDefaultAsync(s => s.Name == name);
    }
}