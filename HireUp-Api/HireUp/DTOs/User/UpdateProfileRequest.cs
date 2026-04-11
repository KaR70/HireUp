namespace HireUp.DTOs.User;

public class UpdateProfileRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int? JobTitleId { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Header { get; set; }
    public string? Bio { get; set; }
}