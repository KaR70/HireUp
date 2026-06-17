using HireUp.Abstraction;
using HireUp.Database.Interfaces;
using HireUp.Dtos.User;
using HireUp.DTOs.User;
using HireUp.Extensions;
using HireUp.Interfaces;
using HireUp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace HireUp.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ISavedJobRepository _savedJobRepository;
    private readonly IUserService _userService;
    private readonly UrlBuilderService _urlBuilderService;
    private readonly INotificationService _notificationService;
    private readonly IJobApplicationService _jobApplicationService;

    public UserController(IUserService userService, UrlBuilderService urlBuilderService, INotificationService notificationService, ISavedJobRepository savedJobRepository, IJobApplicationService jobApplicationService)
    {
        _userService = userService;
        _urlBuilderService = urlBuilderService;
        _savedJobRepository = savedJobRepository;
        _notificationService = notificationService;
        _jobApplicationService = jobApplicationService;
    }

    [HttpGet("me/notifications")]
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
    public async Task<IActionResult> GetMyNotifications()
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _notificationService.GetGroupedNotificationsAsync(userId);
        return Ok(result);
    }

    [HttpPost("notifications/{id}/mark-as-read")]
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result = await _notificationService.MarkAsReadAsync(id);
        return result ? Ok() : NotFound();
    }
    /// <summary>
    /// Retrieves the authenticated user's profile header information.
    /// </summary>
    /// <remarks>
    /// This includes basic information used in the profile header such as name, profile picture URL, etc.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns the authenticated user's profile header information</returns>
    /// <response code="200">Successfully retrieved user profile header</response>
    /// <response code="401">Unauthorized - invalid, expired, or missing JWT token</response>
    /// <response code="404">User profile not found</response>
    [HttpGet("me/profile-header")]
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
    [ProducesResponseType(typeof(ProfileHeaderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileHeader(CancellationToken cancellationToken = default)
    {
        string? userId = User.GetUserId();
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _userService.GetProfileHeaderAsync(userId, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    
    /// <summary>
    /// Retrieves the authenticated user's complete profile information.
    /// </summary>
    /// <remarks>
    /// This endpoint requires a valid JWT Bearer token in the Authorization header.
    /// Returns comprehensive profile details including personal information, location, job title, and profile picture.
    ///
    /// Sample success response (200):
    ///
    ///     {
    ///       "userId": "user-123",
    ///       "email": "john.doe@example.com",
    ///       "fullName": "John Doe",
    ///       "jobTitle": "Senior Software Engineer",
    ///       "birthday": "1990-05-15",
    ///       "gender": "Male",
    ///       "phoneNumber": "+1-555-0123",
    ///       "location": {
    ///         "city": "San Francisco",
    ///         "country": "United States"
    ///       },
    ///       "profilePictureUrl": "https://api.example.com/images/profile/user-123.jpg"
    ///     }
    ///
    /// Sample error response (401 - Unauthorized):
    ///
    ///     {
    ///       "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
    ///       "title": "Unauthorized",
    ///       "status": 401,
    ///       "detail": "Invalid JWT Token",
    ///       "error": ["User.InvalidJwtToken", "The provided JWT Token is invalid"]
    ///     }
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns the authenticated user's complete profile details including personal information and profile picture</returns>
    /// <response code="200">Successfully retrieved user profile with all details</response>
    /// <response code="401">Unauthorized - invalid, expired, or missing JWT token</response>
    /// <response code="404">User profile not found</response>
    [HttpGet("")]
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
    public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken = default)
    {
        string? currentUserId = User.GetUserId();
        if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

        var result = await _userService.GetMyProfileAsync(currentUserId, cancellationToken);
        
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    /// <summary>
    /// Retrieves the public profile information for a specific user.
    /// </summary>
    /// <remarks>
    /// This endpoint is publicly accessible and does not require authentication.
    /// It returns comprehensive public profile information including skills, rating, followers count, and projects count.
    ///
    /// Sample success response (200):
    ///
    ///     {
    ///       "userId": "user-456",
    ///       "fullName": "Jane Smith",
    ///       "header": "Full Stack Developer",
    ///       "aboutMe": "Passionate about building innovative solutions",
    ///       "projectCount": 12,
    ///       "followersCount": 245,
    ///       "rating": 4.8,
    ///       "profilePicture": "https://api.example.com/images/profile/user-456.jpg",
    ///       "skills": [
    ///         {
    ///           "id": 1,
    ///           "name": "React"
    ///         },
    ///         {
    ///           "id": 2,
    ///           "name": ".NET"
    ///         }
    ///       ]
    ///     }
    /// </remarks>
    /// <param name="userId">The unique identifier of the user whose profile to retrieve</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns the user's publicly available profile information</returns>
    /// <response code="200">Successfully retrieved user's public profile</response>
    /// <response code="404">User not found with the specified userId</response>
    [HttpGet("{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserPublicProfile(string userId, CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetUserPublicProfileAsync(userId, cancellationToken);
        
        if (result.IsFailure)
            return result.ToProblem();

        var profile = result.Value;
        profile.ProfilePicture = _urlBuilderService.ToAbsoluteUrl(profile.ProfilePicture);
        return Ok(profile);
    }
    
    /// <summary>
    /// Updates the authenticated user's profile information.
    /// </summary>
    /// <remarks>
    /// Allows updating profile details such as firstName, lastName, bio, location, birthday, phone, gender, and job title.
    /// Returns 200 OK with the updated profile on success.
    ///
    /// Sample request:
    ///
    ///     {
    ///       "firstName": "John",
    ///       "lastName": "Doe",
    ///       "jobTitleId": 1,
    ///       "birthday": "1990-05-15",
    ///       "gender": "Male",
    ///       "phoneNumber": "+1-555-0123",
    ///       "country": "United States",
    ///       "city": "San Francisco",
    ///       "header": "Senior Software Engineer",
    ///       "bio": "Passionate about building scalable applications"
    ///     }
    ///
    /// Sample success response (200 - OK):
    ///
    ///     {
    ///       "userId": "user-123",
    ///       "email": "john.doe@example.com",
    ///       "fullName": "John Doe",
    ///       "jobTitle": "Senior Software Engineer",
    ///       "birthday": "1990-05-15",
    ///       "gender": "Male",
    ///       "phoneNumber": "+1-555-0123",
    ///       "location": {
    ///         "city": "San Francisco",
    ///         "country": "United States"
    ///       },
    ///       "profilePictureUrl": "https://api.example.com/images/profile/user-123.jpg"
    ///     }
    ///
    /// Sample error response (400 - Update failed):
    ///
    ///     {
    ///       "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
    ///       "title": "Bad Request",
    ///       "status": 400,
    ///       "detail": "Could not update user profile.",
    ///       "error": ["Update.Failed", "Could not update user profile."]
    ///     }
    /// </remarks>
    /// <param name="request">The updated profile details (firstName, lastName, jobTitle, birthday, gender, phoneNumber, country, city, header, bio)</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns the updated profile with all details</returns>
    /// <response code="200">Profile updated successfully - returns updated MyProfileResponse</response>
    /// <response code="400">Invalid request format, validation error, or update operation failed</response>
    /// <response code="401">Unauthorized - invalid or missing JWT token</response>
    /// <response code="404">User not found</response>
    /// <response code="422">Validation error in profile data (invalid field values, etc.)</response>
    [HttpPut("me")]
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
    [ProducesResponseType(typeof(MyProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        string? currentUserId = User.GetUserId();

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();
        
        var result = await _userService.UpdateMyProfileAsync(currentUserId, request, cancellationToken);
        
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    /// <summary>
    /// Retrieves the authenticated user's job preferences.
    /// </summary>
    /// <remarks>
    /// Returns the user's preferred job types, office types, locations, and job role.
    ///
    /// Sample success response (200):
    ///
    ///     {
    ///       "jobTypes": [
    ///         { "id": 1, "name": "Full-Time" },
    ///         { "id": 2, "name": "Part-Time" }
    ///       ],
    ///       "officeTypes": [
    ///         { "id": 1, "name": "Remote" },
    ///         { "id": 2, "name": "On-Site" }
    ///       ],
    ///       "locations": [
    ///         { "id": 1, "name": "San Francisco" },
    ///         { "id": 2, "name": "New York" }
    ///       ],
    ///       "jobRole": { "id": 5, "name": "Senior Software Engineer" }
    ///     }
    /// </remarks>
    /// <returns>Returns the user's job preferences</returns>
    /// <response code="200">Successfully retrieved user preferences</response>
    /// <response code="401">Unauthorized - invalid or missing JWT token</response>
    [HttpGet("me/preferences")]
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
    [ProducesResponseType(typeof(UserPreferencesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyPreferences()
    {
        string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

        var preferences = await _userService.GetUserPreferencesAsync(currentUserId);
        return Ok(preferences);
    }

    /// <summary>
    /// Updates the authenticated user's job preferences.
    /// </summary>
    /// <remarks>
    /// Allows updating user's preferred job types, office types, locations, and job role.
    /// Returns 204 No Content on success.
    ///
    /// Sample request:
    ///
    ///     {
    ///       "jobTypeIds": [1, 2, 3],
    ///       "officeTypeIds": [1, 2],
    ///       "locationIds": [1, 3, 5],
    ///       "jobRoleId": 7
    ///     }
    /// </remarks>
    /// <param name="request">The user's updated job preferences (jobTypeIds, officeTypeIds, locationIds, jobRoleId)</param>
    /// <returns>Returns 204 No Content if update was successful</returns>
    /// <response code="204">Preferences updated successfully - no response body returned</response>
    /// <response code="400">Invalid request format or update operation failed</response>
    /// <response code="401">Unauthorized - invalid or missing JWT token</response>
    [HttpPut("me/preferences")]
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateMyPreferences([FromBody] UpdateUserPreferencesRequest request)
    {
        string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

        var result = await _userService.UpdateUserPreferencesAsync(currentUserId, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    /// <summary>
    /// Retrieves the authenticated user's profile picture.
    /// </summary>
    /// <remarks>
    /// This endpoint returns the URL of the user's profile picture.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns the URL of the user's profile picture</returns>
    /// <response code="200">Successfully retrieved profile picture URL</response>
    /// <response code="401">Unauthorized - invalid, expired, or missing JWT token</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("me/profile-picture")]
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
    [ProducesResponseType(typeof(ProfilePictureResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfilePicture(CancellationToken cancellationToken = default)
    {
        string? userId = User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _userService.GetProfilePictureAsync(userId, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
    
    [HttpPost("me/profile-picture")]
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
    public async Task<IActionResult> UpdateProfilePicture([FromForm] ProfilePictureUpload profilePicture, CancellationToken cancellationToken = default)
    {
        string? currentUserId = User.GetUserId();
        if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();
        var result = await _userService.UpdateProfilePictureAsync(currentUserId, profilePicture.Image, cancellationToken);
        if (result.IsFailure) return result.ToProblem();
        var profilePictureUrl = _urlBuilderService.ToAbsoluteUrl(result.Value);
        return Ok(new { profilePictureUrl });
    }

    /// <summary>
    /// استرجاع الوظائف المحفوظة للمستخدم الحالي تلقائياً من التوكن
    /// </summary>
    [HttpGet("me/saved-jobs")]
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")] // لازم يكون عامل Login عشان نقدر نعرف الـ ID بتاعه
    public async Task<IActionResult> GetMySavedJobs()
    {
        // سحب الـ User ID من الـ Claims الموجودة في الـ Token
        string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized("المستخدم غير مصرح له بالوصول.");

        var savedJobs = await _savedJobRepository.GetAllAsync(currentUserId);

        var result = savedJobs.Select(sj => new {
            sj.Id,
            sj.SavedAt,
            JobId = sj.JobListingId,
            JobTitle = sj.JobListing?.Title,
            Company = sj.JobListing?.Company?.Name
        });

        return Ok(result);
    }
    /// <summary>
    /// استرجاع قائمة طلبات التقديم الخاصة بالمستخدم الحالي
    /// </summary>
    [HttpGet("me/applications")]
    [Authorize(Roles = $"{DefaultRoles.Freelancer},{DefaultRoles.DisabledFreelancer}")]
    [ProducesResponseType(typeof(IEnumerable<JobApplicationSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyApplications()
    {
        string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();

        var applications = await _jobApplicationService.GetMyApplicationsAsync(currentUserId);

        foreach (var app in applications)
        {
            app.CompanyLogoUrl = _urlBuilderService.ToAbsoluteUrl(app.CompanyLogoUrl);
        }

        return Ok(applications);
    }

}