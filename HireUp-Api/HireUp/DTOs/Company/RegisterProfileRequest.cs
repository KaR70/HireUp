namespace HireUp.DTOs.Company;

public class RegisterProfileRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int? IndustryId { get; set; }
    public int? LocationId { get; set; }
    public string? Website { get; set; }
    public string? LinkedIn { get; set; }
    public int? FoundedYear{ get; set; }
}