namespace HireUp.Database.Interfaces
{
    public interface ISkillRepository : IRepository<Skill>
    {
        Task<IEnumerable<Skill>> GetSkillsByCategoryAsync(SkillCategory category);
        Task<IEnumerable<Skill>> SearchSkillsAsync(string searchTerm);
        Task<Skill> GetByNameAsync(string name);
    }
}