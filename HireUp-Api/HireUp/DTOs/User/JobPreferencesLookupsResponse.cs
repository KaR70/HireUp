using HireUp.DTOs.Common;

namespace HireUp.DTOs.User; 

public class JobPreferencesLookupsResponse
{
    public IEnumerable<LookupDto> JobTypes { get; set; } = [];
    public IEnumerable<LookupDto> Locations { get; set; } = [];
    public IEnumerable<LookupDto> WorkModes { get; set; } = [];
    public IEnumerable<LookupDto> JobRoles { get; set; } = [];
}