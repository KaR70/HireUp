using HireUp.Abstraction;
using HireUp.DTOs.Location;
using HireUp.DTOs.Industry;
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
    
    /// <summary>
    /// Retrieves all available locations for job search and company registration.
    /// </summary>
    /// <remarks>
    /// Returns a comprehensive list of all locations in the system including city and country information.
    /// This endpoint is publicly accessible and does not require authentication.
    ///
    /// Sample success response (200):
    ///
    ///     [
    ///       {
    ///         "id": 1,
    ///         "city": "San Francisco",
    ///         "country": "United States"
    ///       },
    ///       {
    ///         "id": 2,
    ///         "city": "New York",
    ///         "country": "United States"
    ///       },
    ///       {
    ///         "id": 3,
    ///         "city": "London",
    ///         "country": "United Kingdom"
    ///       }
    ///     ]
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns all available locations</returns>
    /// <response code="200">Successfully retrieved locations list</response>
    /// <response code="404">No locations found</response>
    [HttpGet("locations")]
    [ProducesResponseType(typeof(IEnumerable<LocationSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLocations(CancellationToken cancellationToken)
    {
        var result = await _lookupService.GetLocationsAsync(cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value)
            : result.ToProblem();
    }

    /// <summary>
    /// Retrieves all available industries for company profile and job filtering.
    /// </summary>
    /// <remarks>
    /// Returns a comprehensive list of all industries in the system.
    /// This endpoint is publicly accessible and does not require authentication.
    /// Used primarily for company registration and job preference filtering.
    ///
    /// Sample success response (200):
    ///
    ///     [
    ///       {
    ///         "id": 1,
    ///         "name": "Technology"
    ///       },
    ///       {
    ///         "id": 2,
    ///         "name": "Finance"
    ///       },
    ///       {
    ///         "id": 3,
    ///         "name": "Healthcare"
    ///       },
    ///       {
    ///         "id": 4,
    ///         "name": "Retail"
    ///       }
    ///     ]
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns all available industries</returns>
    /// <response code="200">Successfully retrieved industries list</response>
    /// <response code="404">No industries found</response>
    [HttpGet("industries")]
    [ProducesResponseType(typeof(IEnumerable<IndustryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIndustries(CancellationToken cancellationToken)
    {
        var result = await _lookupService.GetIndustriesAsync(cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value)
            : result.ToProblem();
    }
    
    /// <summary>
    /// Retrieves all available experience levels for job filtering and company recruitment.
    /// </summary>
    /// <remarks>
    /// Returns a comprehensive list of all experience levels in the system.
    /// This endpoint is publicly accessible and does not require authentication.
    /// Used for job posting, job search filters, and candidate profile information.
    ///
    /// Sample success response (200):
    ///
    ///     [
    ///       {
    ///         "id": 1,
    ///         "name": "Entry-Level"
    ///       },
    ///       {
    ///         "id": 2,
    ///         "name": "Junior"
    ///       },
    ///       {
    ///         "id": 3,
    ///         "name": "Mid-Level"
    ///       },
    ///       {
    ///         "id": 4,
    ///         "name": "Senior"
    ///       },
    ///       {
    ///         "id": 5,
    ///         "name": "Lead"
    ///       }
    ///     ]
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns all available experience levels</returns>
    /// <response code="200">Successfully retrieved experience levels list</response>
    [HttpGet("experience-levels")]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExperienceLevels(CancellationToken cancellationToken)
    {
        var result = await _lookupService.GetExperienceLevelsAsync(cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value)
            : result.ToProblem();
    }
}
