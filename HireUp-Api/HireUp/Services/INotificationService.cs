using HireUp.DTOs.Notifications;

namespace HireUp.Services
{
    public interface INotificationService
    {
        Task<GroupedNotificationResponse> GetGroupedNotificationsAsync(string userId);
        Task<bool> MarkAsReadAsync(int id);
    }
}