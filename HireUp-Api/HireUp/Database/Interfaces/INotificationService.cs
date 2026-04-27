using HireUp.DTOs.Notifications;
using HireUp.DTOs.Company;

namespace HireUp.Database.Interfaces {
    public interface INotificationService
    {
        Task<GroupedNotificationResponse> GetGroupedNotificationsAsync(string userId);
        Task<bool> MarkAsReadAsync(int id);

        Task<IEnumerable<CompanyNotificationResponse>> GetCompanyNotificationsAsync(int companyId);
        Task<bool> MarkAllCompanyNotificationsAsReadAsync(int companyId);
    }
}
