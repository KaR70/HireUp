using HireUp.DTOs.JobListing;
using HireUp.Services;
using Microsoft.AspNetCore.Mvc;

namespace HireUp.Controllers;

/// <summary>
/// Provides endpoints for browsing and retrieving job listings.
/// </summary>
[ApiController]
[Route("api/joblistings")]
[Produces("application/json")]
public class JobListingsController : ControllerBase
{
    private readonly IJobListingService _jobListingService;

    public JobListingsController(IJobListingService jobListingService)
    {
        _jobListingService = jobListingService;
    }
    
    /// <summary>
    /// Retrieves all featured job listings.
    /// </summary>
    /// <remarks>
    /// Returns job listings that have been marked as featured by employers.
    /// Featured jobs are highlighted and promoted to attract more applicants.
    /// No authentication required.
    ///
    /// Sample success response (200):
    ///
    ///     [
    ///       {
    ///         "id": 1,
    ///         "title": "Senior C# Developer",
    ///         "companyName": "TechCorp Inc",
    ///         "companyLogoUrl": "https://api.example.com/images/logos/techcorp.jpg",
    ///         "salaryDisplay": "$80,000 - $120,000/year",
    ///         "location": "San Francisco, CA",
    ///         "tags": ["C#", ".NET", "Remote", "Full-Time"]
    ///       }
    ///     ]
    /// </remarks>
    /// <returns>Returns a list of featured job listings</returns>
    /// <response code="200">Successfully retrieved featured jobs</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("featured")]
    [ProducesResponseType(typeof(IEnumerable<JobListingSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFeaturedJobs()
    {
        var result = await _jobListingService.GetFeaturedAsync();

        return result.IsSuccess 
            ? Ok(result.Value)
            : result.ToProblem();
    }
    
    /// <summary>
    /// Retrieves the most popular job listings.
    /// </summary>
    /// <remarks>
    /// Returns the top 10 most popular job listings sorted by view count in descending order.
    /// Popular jobs are those that have received the most user interest and views.
    /// No authentication required.
    ///
    /// Sample success response (200):
    ///
    ///     [
    ///       {
    ///         "id": 5,
    ///         "title": "Junior React Developer",
    ///         "companyName": "WebSolutions Ltd",
    ///         "companyLogoUrl": "https://api.example.com/images/logos/websolutions.jpg",
    ///         "salaryDisplay": "$50,000 - $70,000/year",
    ///         "location": "Remote",
    ///         "tags": ["React", "JavaScript", "Frontend", "Entry-Level"]
    ///       }
    ///     ]
    /// </remarks>
    /// <returns>Returns a list of the 10 most popular job listings</returns>
    /// <response code="200">Successfully retrieved popular jobs</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("popular")]
    [ProducesResponseType(typeof(IEnumerable<JobListingSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPopularJobs()
    {
        var result = await _jobListingService.GetPopularAsync();
        
        return result.IsSuccess 
            ? Ok(result.Value)
            : result.ToProblem();
    }
    
    /// <summary>
    /// Retrieves detailed information about a specific job listing.
    /// </summary>
    /// <remarks>
    /// Returns comprehensive details about a job listing including title, description,
    /// requirements, company information, and other relevant details.
    /// No authentication required.
    ///
    /// Sample success response (200):
    ///
    ///     {
    ///       "id": 42,
    ///       "title": "Full-Stack Developer",
    ///       "companyName": "InnovateTech",
    ///       "companyLogoUrl": "https://api.example.com/images/logos/innovatetech.jpg",
    ///       "salaryDisplay": "$70,000 - $100,000/year",
    ///       "location": "New York, NY",
    ///       "tags": ["Full-Stack", ".NET", "React", "SQL"],
    ///       "description": "We are looking for an experienced full-stack developer...",
    ///       "requirements": "5+ years of experience with .NET and React...",
    ///       "aboutCompany": "InnovateTech is a leading software development company..."
    ///     }
    ///
    /// Sample error response (404 - Not Found):
    ///
    ///     {
    ///       "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
    ///       "title": "Not Found",
    ///       "status": 404,
    ///       "detail": "Job Wasn't Found",
    ///       "error": ["JobListing.NotFound", "Job Wasn't Found"]
    ///     }
    /// </remarks>
    /// <param name="id">The unique identifier of the job listing to retrieve</param>
    /// <returns>Returns detailed information about the specified job listing</returns>
    /// <response code="200">Successfully retrieved job listing details</response>
    /// <response code="404">Job listing not found with the specified ID</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(JobListingDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobById(int id)
    {
        var result = await _jobListingService.GetByIdAsync(id);
        
        return result.IsSuccess 
            ? Ok(result.Value)
            : result.ToProblem();
    }
}