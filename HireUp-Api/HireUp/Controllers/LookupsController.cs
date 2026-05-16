using HireUp.Abstraction;
using HireUp.DTOs.Common;
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
    
    /// <summary>
    /// Retrieves all available accessibility needs for disabled users.
    /// </summary>
    /// <remarks>
    /// Returns a comprehensive list of accessibility needs that disabled candidates can specify when registering.
    /// This endpoint is publicly accessible and does not require authentication.
    /// Used to populate accessibility needs selection during user registration and profile management.
    ///
    /// Sample success response (200):
    ///
    ///     [
    ///       {
    ///         "id": 1,
    ///         "name": "Screen-Reader Compatible Software",
    ///         "description": "Requires company internal tools, software systems, and portals to be fully compatible with software like JAWS, NVDA, or VoiceOver."
    ///       },
    ///       {
    ///         "id": 2,
    ///         "name": "High-Contrast UI & Document Formats",
    ///         "description": "Requires digital documentation, corporate handbooks, and operational dashboards to support dark themes and font size scalability."
    ///       },
    ///       {
    ///         "id": 3,
    ///         "name": "Asynchronous Text-First Communication",
    ///         "description": "Prefers business communication, daily updates, and feedback to take place via Slack, Teams, or email rather than audio/video calling."
    ///       },
    ///       {
    ///         "id": 4,
    ///         "name": "Live Meeting Closed-Captioning",
    ///         "description": "Requires all company-wide, all-hands meetings or team syncs to provide real-time automated or manual closed captions."
    ///       },
    ///       {
    ///         "id": 5,
    ///         "name": "Keyboard-Only Digital Navigation",
    ///         "description": "Requires developer environments, corporate web tools, and command interfaces to be fully operational without forcing standard mouse use."
    ///       },
    ///       {
    ///         "id": 6,
    ///         "name": "Accessible Physical Workspace / Ergonomic Equipment",
    ///         "description": "For hybrid roles: Requires step-free physical premises or company-provided ergonomic accessories (e.g., split keyboards, vertical mice, voice-to-text software)."
    ///       },
    ///       {
    ///         "id": 7,
    ///         "name": "Explicit, Written Project Requirements",
    ///         "description": "Requires clear, unambiguous written briefs and ticket documentation rather than loose verbal instructions to optimize workflow execution."
    ///       },
    ///       {
    ///         "id": 8,
    ///         "name": "Flexible Task Breaks & Core Hour Buffers",
    ///         "description": "Requires structural freedom to organize daily focus hours and take micro-breaks to effectively navigate focus and energy variations."
    ///       },
    ///       {
    ///         "id": 9,
    ///         "name": "Flexible Medical Appointment Leave",
    ///         "description": "Requires the allowance to attend routine medical follow-ups, therapy, or checkups during business hours without formal disciplinary tracking."
    ///       }
    ///     ]
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns all available accessibility needs</returns>
    /// <response code="200">Successfully retrieved accessibility needs list</response>
    /// <response code="404">No accessibility needs found</response>
    [HttpGet("accessibility-needs")]
    [ProducesResponseType(typeof(IEnumerable<LookupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccessibilityNeeds(CancellationToken cancellationToken)
    {
        var result = await _lookupService.GetAccessibilityNeedsAsync(cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value)
            : result.ToProblem();
    }
    
    /// <summary>
    /// Retrieves all available disability types for disabled users.
    /// </summary>
    /// <remarks>
    /// Returns a comprehensive list of disability types that candidates can select when registering as a disabled user.
    /// This endpoint is publicly accessible and does not require authentication.
    /// Used to populate disability type selection during user registration and profile management.
    ///
    /// Sample success response (200):
    ///
    ///     [
    ///       {
    ///         "id": 1,
    ///         "name": "Visual Disability",
    ///         "description": "Candidates who are blind, have low vision, or experience color-blindness and utilize assistive visual tools or screen modifications."
    ///       },
    ///       {
    ///         "id": 2,
    ///         "name": "Hearing or Auditory Disability",
    ///         "description": "Candidates who are deaf or hard-of-hearing and rely on written communication channels or captioning frameworks."
    ///       },
    ///       {
    ///         "id": 3,
    ///         "name": "Physical or Mobility Disability",
    ///         "description": "Candidates with limited fine motor skills, repetitive strain injuries, or mobility variations requiring ergonomic or alternative physical/digital access."
    ///       },
    ///       {
    ///         "id": 4,
    ///         "name": "Neurodivergence & Cognitive Variations",
    ///         "description": "Candidates with ADHD, Autism, Dyslexia, or processing variations who thrive with structured workflows, clear communication, or specialized environments."
    ///       },
    ///       {
    ///         "id": 5,
    ///         "name": "Chronic Illness or Invisible Disability",
    ///         "description": "Candidates managing long-term health conditions (e.g., autoimmune, chronic pain) requiring energy management or medical schedule flexibility."
    ///       }
    ///     ]
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns all available disability types</returns>
    /// <response code="200">Successfully retrieved disability types list</response>
    /// <response code="404">No disability types found</response>
    [HttpGet("disability-types")]
    [ProducesResponseType(typeof(IEnumerable<LookupDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDisabilityTypes(CancellationToken cancellationToken)
    {
        var result = await _lookupService.GetDisabilityTypesAsync(cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value)
            : result.ToProblem();
    }
}
