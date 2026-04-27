using System;

namespace HireUp.Core.Entities
{
    public enum NotificationCategory
    {
        NewActivity,
        Applications,
        Interview
    }

    public class Notification
    {
        public int Id { get; set; } 
        public string UserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public NotificationCategory Category { get; set; }
        public int? CompanyId { get; set; }
        public string LinkUrl { get; set; }
        public string Title { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }
}