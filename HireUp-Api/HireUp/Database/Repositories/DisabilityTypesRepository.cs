using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories;

public class DisabilityTypesRepository : BaseRepository<DisabilityType>, IDisabilityTypesRepository
{
    public DisabilityTypesRepository(ApplicationDbContext context) : base(context)
    {
    }
}