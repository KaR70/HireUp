using HireUp.Extensions;
using HireUp.Services;
using HireUp.DTOs.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireUp.Controllers;

/// <summary>
/// Provides endpoints for company-related operations including company profile and home dashboard data.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize(Roles = DefaultRoles.Company)]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    /// <summary>
    /// Retrieves the company home dashboard with key metrics and recent applicants.
    /// </summary>
    /// <remarks>
    /// Returns comprehensive dashboard data for the authenticated company user including:
    /// - Company name and logo
    /// - Count of active job listings
    /// - Total number of applicants across all jobs
    /// - List of 4 most recent applicants with their profile information
    ///
    /// This endpoint requires company authentication (user must be registered as a company).
    ///
    /// Sample success response (200):
    ///
    ///     {
    ///       "companyName": "TechCorp Inc",
    ///       "companyLogoUrl": "https://api.example.com/images/logos/techcorp.jpg",
    ///       "activeJobsCount": 12,
    ///       "totalApplicantsCount": 156,
    ///       "recentApplicants": [
    ///         {
    ///           "id": 1,
    ///           "applicantName": "John Doe",
    ///           "applicantProfilePictureUrl": "https://api.example.com/images/profile/john-doe.jpg",
    ///           "appliedJobTitle": "Senior C# Developer",
    ///           "appliedAt": "2024-01-15T14:30:00Z"
    ///         },
    ///         {
    ///           "id": 2,
    ///           "applicantName": "Jane Smith",
    ///           "applicantProfilePictureUrl": "https://api.example.com/images/profile/jane-smith.jpg",
    ///           "appliedJobTitle": "Full Stack Developer",
    ///           "appliedAt": "2024-01-15T10:15:00Z"
    ///         }
    ///       ]
    ///     }
    ///
    /// Sample error response (404 - Company not found):
    ///
    ///     {
    ///       "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
    ///       "title": "Not Found",
    ///       "status": 404,
    ///       "detail": "Company not found",
    ///       "error": ["Company.NotFound", "Company not found"]
    ///     }
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns the company home dashboard data</returns>
    /// <response code="200">Successfully retrieved company home dashboard data</response>
    /// <response code="401">Unauthorized - invalid or missing JWT token</response>
    /// <response code="404">Company not found for the authenticated user</response>
    [HttpGet("home")]
    [ProducesResponseType(typeof(CompanyHomeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHome(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        
        var result = await _companyService.GetHomeAsync(userId, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}

