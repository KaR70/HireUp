using HireUp.DTOs.Common;
using HireUp.DTOs.Location;

namespace HireUp.DTOs.Company;

public class ProfileResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Logo { get; set; }
    public string? Website { get; set; }
    public string? LinkedIn { get; set; }
    public int? FoundedYear { get; set; }
    public LookupDto? Industry { get; set; }
    public LocationSummaryResponse? Headquarters { get; set; }
}