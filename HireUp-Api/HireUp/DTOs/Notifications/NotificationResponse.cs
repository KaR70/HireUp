public class NotificationResponse
{
    public int Id { get; set; }
    public string CompanyLogoUrl { get; set; } 
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public string LinkUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}