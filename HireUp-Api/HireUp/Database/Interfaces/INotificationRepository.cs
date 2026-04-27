using HireUp.Core.Entities;
using HireUp.Database.Repositories; 

namespace HireUp.Database.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<List<Notification>> GetUserNotificationsAsync(string userId);
        Task<Notification?> GetByIdAsync(int id);
        Task UpdateAsync(Notification notification);
        Task<IEnumerable<Notification>> GetCompanyNotificationsAsync(int companyId);

        Task MarkAllAsReadAsync(int companyId);
    }
}