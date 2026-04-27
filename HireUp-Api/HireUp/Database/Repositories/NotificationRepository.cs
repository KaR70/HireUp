using HireUp.Core.Entities;
using HireUp.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireUp.Database.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Notification>> GetCompanyNotificationsAsync(int companyId)
        {
            return await _context.Notifications
                .Where(n => n.CompanyId == companyId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAllAsReadAsync(int companyId)
        {
            var unreadNotifications = await _context.Notifications
                .Where(n => n.CompanyId == companyId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }

    }
}