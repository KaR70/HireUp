namespace HireUp.Entities;

public class Location
{
    public int Id { get; set; }
    public string Country { get; set; }
    public string City { get; set; }

    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}