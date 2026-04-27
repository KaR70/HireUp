namespace HireUp.DTOs.Company
{
    public class CompanyNotificationResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsNew { get; set; } 
        public DateTime CreatedAt { get; set; }
        public string? LinkUrl { get; set; }
    }
}