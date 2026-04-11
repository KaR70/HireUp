using HireUp.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace HireUp.Controllers;

[Route("api/lookups")]
[ApiController]
public class LookupsController : ControllerBase
{
    private readonly ILookupService _lookupService;

    public LookupsController(ILookupService lookupService)
    {
        _lookupService = lookupService;
    }

    [HttpGet("job-preferences")]
    public async Task<IActionResult> GetJobPreferencesLookups()
    {
        var lookups = await _lookupService.GetJobPreferencesLookupsAsync();
        return Ok(lookups);
    }
}