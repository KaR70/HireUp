using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories;

public class LocationRepository : BaseRepository<Location>, ILocationRepository
{
    public LocationRepository(ApplicationDbContext context) : base(context)
    {
    }
}