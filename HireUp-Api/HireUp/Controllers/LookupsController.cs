using HireUp.Abstraction;
using HireUp.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace HireUp.Controllers;

/// <summary>
/// Provides endpoints for retrieving lookup data used for form dropdowns and filters.
/// </summary>
[Route("api/lookups")]
[ApiController]
[Produces("application/json")]
public class LookupsController : ControllerBase
{
    private readonly ILookupService _lookupService;

    public LookupsController(ILookupService lookupService)
    {
        _lookupService = lookupService;
    }

    /// <summary>
    /// Retrieves all available job preference lookup data.
    /// </summary>
    /// <remarks>
    /// Returns comprehensive lookup data needed for job preferences UI including job types, locations, office/work modes, and job roles.
    /// This endpoint is publicly accessible and does not require authentication.
    ///
    /// Sample success response (200):
    ///
    ///     {
    ///       "jobTypes": [
    ///         { "id": 1, "name": "Full-Time" },
    ///         { "id": 2, "name": "Part-Time" },
    ///         { "id": 3, "name": "Contract" },
    ///         { "id": 4, "name": "Freelance" }
    ///       ],
    ///       "locations": [
    ///         { "id": 1, "name": "San Francisco" },
    ///         { "id": 2, "name": "New York" },
    ///         { "id": 3, "name": "London" }
    ///       ],
    ///       "workModes": [
    ///         { "id": 1, "name": "Remote" },
    ///         { "id": 2, "name": "On-Site" },
    ///         { "id": 3, "name": "Hybrid" }
    ///       ],
    ///       "jobRoles": [
    ///         { "id": 1, "name": "Junior Developer" },
    ///         { "id": 2, "name": "Senior Developer" },
    ///         { "id": 3, "name": "Team Lead" }
    ///       ]
    ///     }
    /// </remarks>
    /// <returns>Returns all job preference lookup data</returns>
    /// <response code="200">Successfully retrieved job preferences lookup data</response>
    [HttpGet("job-preferences")]
    [ProducesResponseType(typeof(JobPreferencesLookupsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJobPreferencesLookups()
    {
        var lookups = await _lookupService.GetJobPreferencesLookupsAsync();
        return Ok(lookups);
    }
}
