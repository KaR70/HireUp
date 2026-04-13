namespace HireUp.DTOs.Notifications
{
    public class GroupedNotificationResponse
    {
        public List<NotificationResponse> NewActivity { get; set; } = new();
        public List<NotificationResponse> Applications { get; set; } = new();
        public List<NotificationResponse> Interview { get; set; } = new();
    }
}
