namespace HireUp.DTOs.Authentication;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<int> SelectedJobTypeIds { get; set; } = new List<int>();
    public List<int> SelectedLocationIds { get; set; } = new List<int>();
}