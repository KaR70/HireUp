namespace HireUp.DTOs.User;
using HireUp.DTOs.Common;

public class UserPreferencesResponse
{
    public List<LookupDto> JobTypes { get; set; } = new();
    public List<LookupDto> OfficeTypes { get; set; } = new();
    public List<LookupDto> Locations { get; set; } = new();
    
    //TODO: Decide wether to keep this or not
    //public List<LookupDto> JobCategories { get; set; } = new();
    
    public LookupDto? JobRole { get; set; }
}