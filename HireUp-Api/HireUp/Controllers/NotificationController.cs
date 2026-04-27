using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HireUp.DTOs.Company;
using HireUp.DTOs.Notifications;

namespace HireUp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly HireUp.Database.Interfaces.INotificationService _notificationService;

        public NotificationController(HireUp.Database.Interfaces.INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetCompanyNotifications(int companyId)
        {
            var notifications = await _notificationService.GetCompanyNotificationsAsync(companyId);
            return Ok(notifications);
        }

        [HttpPost("company/{companyId}/mark-all-read")]
        public async Task<IActionResult> MarkAllCompanyAsRead(int companyId)
        {
            await _notificationService.MarkAllCompanyNotificationsAsReadAsync(companyId);
            return Ok(new { message = "All notifications marked as read." });
        }

        [HttpPost("{id}/mark-as-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _notificationService.MarkAsReadAsync(id);
            if (!result) return NotFound();

            return Ok(new { message = "Notification marked as read." });
        }
    }
}