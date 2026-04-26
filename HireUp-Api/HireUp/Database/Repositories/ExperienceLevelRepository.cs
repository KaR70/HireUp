using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories;

public class ExperienceLevelRepository : BaseRepository<Industry>, IExperienceLevelRepository
{
    public ExperienceLevelRepository(ApplicationDbContext context) : base(context)
    {
    }
}