using HireUp.Core.Entities;
using HireUp.Database.Interfaces;
using HireUp.DTOs.Notifications;
using HireUp.DTOs.Company; 

namespace HireUp.Services
{
    public class NotificationService : HireUp.Database.Interfaces.INotificationService
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


        public async Task<IEnumerable<CompanyNotificationResponse>> GetCompanyNotificationsAsync(int companyId)
        {
            var notifications = await _notificationRepo.GetCompanyNotificationsAsync(companyId);

            return notifications.Select(MapToCompanyDto);
        }

        public async Task<bool> MarkAllCompanyNotificationsAsReadAsync(int companyId)
        {
            await _notificationRepo.MarkAllAsReadAsync(companyId);
            return true;
        }


        private NotificationResponse MapToDto(Notification n) => new NotificationResponse
        {
            Id = n.Id,
            Message = n.Message,
            CreatedAt = n.CreatedAt,
            IsRead = n.IsRead
        };

        private CompanyNotificationResponse MapToCompanyDto(Notification n) => new CompanyNotificationResponse
        {
            Id = n.Id,
            Title = n.Title,
            Message = n.Message,
            IsNew = !n.IsRead, 
            CreatedAt = n.CreatedAt,
            LinkUrl = n.LinkUrl
        };
    }
}