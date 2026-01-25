using Microsoft.EntityFrameworkCore;
using HireUp.Database.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HireUp.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersWithDisabilitiesAsync()
            => await _context.Users
                .AsNoTracking()
                .Where(u => u.UserDisabilityTypes.Any() && u.IsActive)
                .Include(u => u.UserDisabilityTypes)
                    .ThenInclude(udt => udt.DisabilityType)
                .Include(u => u.Skills)
                .ToListAsync();

        //public async Task<IEnumerable<ApplicationUser>> GetAvailableInterviewersAsync()
        //    => await _dbSet.Where(u => u.Type == UserType.Interviewer && u.IsActive).ToListAsync();

    }
}