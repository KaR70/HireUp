using HireUp.Database.Interfaces;
using HireUp.DTOs;
using HireUp.DTOs.JobListing;
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
    [AllowAnonymous]
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
    [AllowAnonymous]
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
    /// - Company information and logo
    /// - Location, salary, and job type
    /// - Accessibility needs and accommodations available
    /// - Tags describing job features
    ///
    /// This endpoint is publicly accessible and does not require authentication.
    ///
    /// Sample success response (200):
    ///
    ///     {
    ///       "id": 42,
    ///       "title": "Senior C# Developer",
    ///       "description": "We are looking for an experienced C# developer to join our team and work on cutting-edge enterprise solutions...",
    ///       "requirements": "5+ years of experience with C#, .NET, and SQL Server. Strong knowledge of cloud technologies (Azure) preferred.",
    ///       "aboutCompany": "TechCorp Inc is a leading software development company specializing in enterprise solutions for Fortune 500 companies.",
    ///       "companyName": "TechCorp Inc",
    ///       "companyLogoUrl": "https://example.com/logos/techcorp.png",
    ///       "salaryDisplay": "$150,000 - $200,000",
    ///       "location": "San Francisco, CA",
    ///       "tags": [
    ///         "Remote-Friendly",
    ///         "Screen-Reader Compatible",
    ///         "Flexible Schedule"
    ///       ],
    ///       "accessibilityNeeds": [
    ///         "Screen-Reader Compatible Software",
    ///         "Asynchronous Text-First Communication",
    ///         "Flexible Task Breaks & Core Hour Buffers"
    ///       ]
    ///     }
    /// </remarks>
    /// <param name="id">The unique identifier of the job listing</param>
    /// <returns>Returns detailed job listing information including accessibility needs and accommodations</returns>
    /// <response code="200">Successfully retrieved job listing details</response>
    /// <response code="404">Job listing not found with the specified ID</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
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
    [Authorize(Roles = DefaultRoles.Company)]
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
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
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
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
    public async Task<IActionResult> UnsaveJob(int id, [FromQuery] string userId)
    {
        await _savedJobRepository.RemoveAsync(id, userId);
        return Ok(new { message = "تمت إزالة الوظيفة من المحفوظات" });
    }

    /// <summary>
    /// Updates an existing job listing posted by the authenticated company.
    /// </summary>
    /// <remarks>
    /// Allows company users to modify job listing details such as title, description, salary, requirements, and more.
    /// Only the company that posted the job can update it.
    /// Returns 204 No Content on success with no response body.
    ///
    /// Sample request:
    ///
    ///     {
    ///       "title": "Senior Full Stack Developer",
    ///       "description": "We are looking for an experienced full stack developer to join our team...",
    ///       "requirements": "7+ years of experience with .NET and React, strong database design skills",
    ///       "salary": 180000,
    ///       "isInclusiveHiring": true,
    ///       "disabilitySupport": "Flexible work hours and remote-first option available",
    ///       "expiryDate": "2024-03-31",
    ///       "isActive": true,
    ///       "experienceLevelId": 4,
    ///       "jobRoleId": 4,
    ///       "locationId": 1
    ///     }
    /// </remarks>
    /// <param name="id">The unique identifier of the job listing to update</param>
    /// <param name="request">The updated job listing details</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns 204 No Content if update was successful</returns>
    /// <response code="204">Job listing updated successfully - no response body returned</response>
    /// <response code="401">Unauthorized - invalid or missing JWT token</response>
    /// <response code="403">Forbidden - user is not authorized to update this job listing</response>
    /// <response code="404">Job listing not found</response>
    [HttpPut("{id}")]
    [Authorize(Roles = DefaultRoles.Company)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRequest request,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _jobListingService.UpdateAsync(userId, id, request, cancellationToken);
        
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    /// <summary>
    /// Deletes a job listing posted by the authenticated company.
    /// </summary>
    /// <remarks>
    /// Allows company users to delete job listings they have posted.
    /// Only the company that posted the job can delete it.
    /// Deleted job listings will no longer be visible to job seekers.
    /// Returns 204 No Content on success with no response body.
    ///
    /// Sample error response (403 - Forbidden):
    ///
    ///     {
    ///       "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
    ///       "title": "Forbidden",
    ///       "status": 403,
    ///       "detail": "You are not authorized to delete this job listing",
    ///       "error": ["JobListing.Unauthorized", "You are not authorized to delete this job listing"]
    ///     }
    /// </remarks>
    /// <param name="id">The unique identifier of the job listing to delete</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns 204 No Content if deletion was successful</returns>
    /// <response code="204">Job listing deleted successfully - no response body returned</response>
    /// <response code="401">Unauthorized - invalid or missing JWT token</response>
    /// <response code="403">Forbidden - user is not authorized to delete this job listing</response>
    /// <response code="404">Job listing not found</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = DefaultRoles.Company)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        
        var result = await _jobListingService.DeleteAsync(userId, id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
    
    /// <summary>
    /// Searches for job listings with advanced filtering options.
    /// </summary>
    /// <remarks>
    /// Returns job listings that match the specified search criteria and filters.
    /// This endpoint supports comprehensive job search with multiple filter combinations.
    /// Jobs are matched based on all provided filter criteria.
    ///
    /// Supported Filter Parameters:
    /// - searchTerm: Search in job title and description
    /// - accessibilityNeedIds: Filter by accessibility needs (IDs: 1-9)
    /// - experienceLevelId: Filter by experience level required
    /// - jobTypeId: Filter by job type (Full-Time, Part-Time, Contract, etc.)
    /// - locationId: Filter by job location
    /// - officeTypeId: Filter by office type (Remote, On-Site, Hybrid)
    /// - industryId: Filter by company industry
    ///
    /// Accessibility Need IDs:
    /// - 1: Screen-Reader Compatible Software
    /// - 2: High-Contrast UI & Document Formats
    /// - 3: Asynchronous Text-First Communication
    /// - 4: Live Meeting Closed-Captioning
    /// - 5: Keyboard-Only Digital Navigation
    /// - 6: Accessible Physical Workspace / Ergonomic Equipment
    /// - 7: Explicit, Written Project Requirements
    /// - 8: Flexible Task Breaks & Core Hour Buffers
    /// - 9: Flexible Medical Appointment Leave
    ///
    /// Sample request (multiple filters):
    ///
    ///     GET /api/joblistings/search?searchTerm=developer&accessibilityNeedIds=1&accessibilityNeedIds=3&experienceLevelId=4&locationId=1&jobTypeId=1
    ///
    /// Sample request (accessibility needs only):
    ///
    ///     GET /api/joblistings/search?accessibilityNeedIds=1&accessibilityNeedIds=5
    ///
    /// Sample success response (200):
    ///
    ///     [
    ///       {
    ///         "id": 12,
    ///         "title": "Senior Full Stack Developer",
    ///         "companyName": "TechCorp Inc",
    ///         "companyLogoUrl": "https://example.com/logos/techcorp.png",
    ///         "salaryDisplay": "$150,000 - $200,000",
    ///         "location": "Remote",
    ///         "jobTypeId": 1,
    ///         "experienceLevelId": 4,
    ///         "tags": [
    ///           "Screen-Reader Compatible",
    ///           "Text-First Communication",
    ///           "Keyboard Navigation"
    ///         ],
    ///         "postedAt": "2024-01-15T10:30:00Z"
    ///       },
    ///       {
    ///         "id": 18,
    ///         "title": "Backend Developer",
    ///         "companyName": "WebSolutions Ltd",
    ///         "companyLogoUrl": "https://example.com/logos/websolutions.png",
    ///         "salaryDisplay": "$120,000 - $160,000",
    ///         "location": "San Francisco, CA",
    ///         "jobTypeId": 1,
    ///         "experienceLevelId": 3,
    ///         "tags": [
    ///           "Remote-Friendly",
    ///           "Flexible Schedule"
    ///         ],
    ///         "postedAt": "2024-01-14T14:20:00Z"
    ///       }
    ///     ]
    ///
    /// Sample empty response (200 - No matching jobs found):
    ///
    ///     []
    /// </remarks>
    /// <param name="filters">Search and filter criteria including searchTerm, accessibility needs, experience level, job type, location, office type, and industry</param>
    /// <param name="ct">Cancellation token for the async operation</param>
    /// <returns>Returns a list of job listings matching all specified criteria</returns>
    /// <response code="200">Successfully retrieved matching jobs list (may be empty if no matches found)</response>
    /// <response code="400">Invalid filter parameters or request format</response>
    [HttpGet("search")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<JobListingSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchJobs([FromQuery] JobSearchFilterDto filters, CancellationToken ct)
    {
        var result = await _jobListingService.SearchJobsAsync(filters, ct);
    
        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem();
    }
}