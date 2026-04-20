using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories;

public class IndustryRepository : BaseRepository<Industry>, IIndustryRepository
{
    public IndustryRepository(ApplicationDbContext context) : base(context)
    {
    }
}