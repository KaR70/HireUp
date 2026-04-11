namespace HireUp.DTOs.User;
using HireUp.DTOs.Common;

public class UserPreferencesResponse
{
    public List<LookupDto> JobTypes { get; set; } = new();
    public List<LookupDto> OfficeTypes { get; set; } = new();
    public List<LookupDto> Locations { get; set; } = new();
    public List<LookupDto> JobCategories { get; set; } = new();
    public List<LookupDto> JobRoles { get; set; } = new();
}