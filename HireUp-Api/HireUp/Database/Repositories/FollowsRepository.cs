using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories;

public class FollowsRepository : BaseRepository<Follows>, IFollowsRepository
{
    public FollowsRepository(ApplicationDbContext context) : base(context)
    {
    }
}