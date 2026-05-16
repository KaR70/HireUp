using HireUp.Database.Interfaces;

namespace HireUp.Database.Repositories;

public class AccessibilityNeedsRepository : BaseRepository<AccessibilityNeed>, IAccessibilityNeedsRepository
{
    public AccessibilityNeedsRepository(ApplicationDbContext context) : base(context)
    {
        
    }
}