using HireUp.Core.Entities;
using HireUp.Database.Interfaces;
using HireUp.DTOs.Notifications;

namespace HireUp.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepo;

        public NotificationService(INotificationRepository notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }

        public async Task<GroupedNotificationResponse> GetGroupedNotificationsAsync(string userId)
        {
            var allNotifications = await _notificationRepo.GetUserNotificationsAsync(userId);

            return new GroupedNotificationResponse
            {
                NewActivity = allNotifications.Where(n => n.Category == NotificationCategory.NewActivity).Select(MapToDto).ToList(),
                Applications = allNotifications.Where(n => n.Category == NotificationCategory.Applications).Select(MapToDto).ToList(),
                Interview = allNotifications.Where(n => n.Category == NotificationCategory.Interview).Select(MapToDto).ToList()
            };
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _notificationRepo.GetByIdAsync(id);
            if (notification == null) return false;

            notification.IsRead = true;
            await _notificationRepo.UpdateAsync(notification);
            return true;
        }

        private NotificationResponse MapToDto(Notification n) => new NotificationResponse
        {
            Id = n.Id,
            Message = n.Message,
            CreatedAt = n.CreatedAt,
            IsRead = n.IsRead
        };
    }
}