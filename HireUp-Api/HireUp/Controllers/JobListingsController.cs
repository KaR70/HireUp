using HireUp.Database.Interfaces;
using HireUp.DTOs.JobListing;
using HireUp.Entities;
using HireUp.Extensions; // تم إضافة هذا السطر ليتعرف على SavedJob
using HireUp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HireUp.Controllers;

/// <summary>
/// Provides endpoints for job listing operations including browsing, searching, and saving job listings.
/// </summary>
[ApiController]
[Route("api/joblistings")]
[Produces("application/json")]
public class JobListingsController : ControllerBase
{
    private readonly ISavedJobRepository _savedJobRepository;
    private readonly IJobListingService _jobListingService;

    // تم تعديل الـ Constructor لاستقبال الـ Repository وحل مشكلة الـ NullReferenceException
    public JobListingsController(IJobListingService jobListingService, ISavedJobRepository savedJobRepository)
    {
        _jobListingService = jobListingService;
        _savedJobRepository = savedJobRepository;
    }

    /// <summary>
    /// Retrieves all featured job listings.
    /// </summary>
    /// <remarks>
    /// Returns a list of job listings that have been marked as featured by the company.
    /// Featured jobs appear prominently in the job board and are highlighted for visibility.
    /// This endpoint is publicly accessible and does not require authentication.
    ///
    /// Sample success response (200):
    ///
    ///     [
    ///       {
    ///         "id": 1,
    ///         "title": "Senior C# Developer",
    ///         "companyName": "TechCorp Inc",
    ///         "location": "San Francisco, CA",
    ///         "jobTypeId": 1,
    ///         "experienceLevelId": 4,
    ///         "postedAt": "2024-01-10T08:30:00Z"
    ///       },
    ///       {
    ///         "id": 2,
    ///         "title": "Full Stack Developer",
    ///         "companyName": "WebSolutions Ltd",
    ///         "location": "New York, NY",
    ///         "jobTypeId": 1,
    ///         "experienceLevelId": 3,
    ///         "postedAt": "2024-01-12T14:15:00Z"
    ///       }
    ///     ]
    /// </remarks>
    /// <returns>Returns a list of featured job listings</returns>
    /// <response code="200">Successfully retrieved featured jobs list</response>
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
    /// Retrieves the most popular job listings based on view count.
    /// </summary>
    /// <remarks>
    /// Returns the top 10 most viewed job listings on the platform.
    /// Popularity is determined by the number of views each job listing has received.
    /// This endpoint is publicly accessible and does not require authentication.
    ///
    /// Sample success response (200):
    ///
    ///     [
    ///       {
    ///         "id": 5,
    ///         "title": "Data Scientist",
    ///         "companyName": "AI Corp",
    ///         "location": "Boston, MA",
    ///         "jobTypeId": 1,
    ///         "experienceLevelId": 4,
    ///         "postedAt": "2024-01-08T11:20:00Z"
    ///       },
    ///       {
    ///         "id": 7,
    ///         "title": "DevOps Engineer",
    ///         "companyName": "CloudTech Inc",
    ///         "location": "Remote",
    ///         "jobTypeId": 1,
    ///         "experienceLevelId": 3,
    ///         "postedAt": "2024-01-09T15:45:00Z"
    ///       }
    ///     ]
    /// </remarks>
    /// <returns>Returns a list of the most popular job listings (top 10)</returns>
    /// <response code="200">Successfully retrieved popular jobs list</response>
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
    /// Returns comprehensive details for a single job listing including:
    /// - Job title, description, and requirements
    /// - Company information
    /// - Location and job type
    /// - Experience level and salary (if available)
    /// - Application deadline
    ///
    /// This endpoint is publicly accessible and does not require authentication.
    ///
    /// Sample success response (200):
    ///
    ///     {
    ///       "id": 42,
    ///       "title": "Senior C# Developer",
    ///       "description": "We are looking for an experienced C# developer...",
    ///       "requirements": "5+ years of experience with C#, .NET, and SQL Server",
    ///       "companyName": "TechCorp Inc",
    ///       "location": "San Francisco, CA",
    ///       "salary": 150000,
    ///       "jobTypeId": 1,
    ///       "experienceLevelId": 4,
    ///       "postedAt": "2024-01-10T08:30:00Z",
    ///       "expiryDate": "2024-02-10T08:30:00Z"
    ///     }
    /// </remarks>
    /// <param name="id">The unique identifier of the job listing</param>
    /// <returns>Returns detailed job listing information</returns>
    /// <response code="200">Successfully retrieved job listing details</response>
    /// <response code="404">Job listing not found with the specified ID</response>
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

    /// <summary>
    /// Retrieves all job listings posted by the authenticated company user.
    /// </summary>
    /// <remarks>
    /// Returns a list of all job postings created by the authenticated company, including:
    /// - Job listing ID and title
    /// - Posted date/time
    /// - Active status
    /// - Number of applicants for each job
    ///
    /// This endpoint requires company authentication (JWT token).
    /// The company is automatically identified from the authenticated user's ID.
    ///
    /// Sample success response (200):
    ///
    ///     [
    ///       {
    ///         "id": 1,
    ///         "title": "Senior C# Developer",
    ///         "postedAt": "2024-01-10T08:30:00Z",
    ///         "isActive": true,
    ///         "applicantsCount": 24
    ///       },
    ///       {
    ///         "id": 2,
    ///         "title": "Full Stack Developer",
    ///         "postedAt": "2024-01-12T14:15:00Z",
    ///         "isActive": true,
    ///         "applicantsCount": 18
    ///       },
    ///       {
    ///         "id": 3,
    ///         "title": "UI/UX Designer",
    ///         "postedAt": "2024-01-05T10:00:00Z",
    ///         "isActive": false,
    ///         "applicantsCount": 12
    ///       }
    ///     ]
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
    /// <returns>Returns a list of job postings created by the company</returns>
    /// <response code="200">Successfully retrieved company's posted jobs list</response>
    /// <response code="401">Unauthorized - invalid or missing JWT token</response>
    /// <response code="404">Company not found for the authenticated user</response>
    [HttpGet("posted-jobs")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<CompanyJobSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPostedJobs(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        
        var result = await _jobListingService.GetCompanyJobSummariesAsync(userId, cancellationToken);
        
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("{id}/save")]
    public async Task<IActionResult> SaveJob(int id, [FromQuery] string userId)
    {
        // التأكد من أن الوظيفة غير محفوظة مسبقاً لهذا المستخدم
        if (await _savedJobRepository.IsAlreadySavedAsync(id, userId))
            return BadRequest("هذه الوظيفة محفوظة بالفعل في قائمتك.");

        var savedJob = new SavedJob
        {
            JobListingId = id,
            UserId = userId,
            SavedAt = DateTime.UtcNow
        };

        await _savedJobRepository.AddAsync(savedJob);
        return Ok(new { message = "تم حفظ الوظيفة بنجاح" });
    }

    
    [HttpDelete("{id}/unsave")]
    public async Task<IActionResult> UnsaveJob(int id, [FromQuery] string userId)
    {
        await _savedJobRepository.RemoveAsync(id, userId);
        return Ok(new { message = "تمت إزالة الوظيفة من المحفوظات" });
    }
    
    
}