using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HireUp.DTOs.User;
using HireUp.Mapping;
using HireUp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace HireUp.Controllers;

/// <summary>
/// Provides endpoints for user profile management including viewing, updating, and managing profile pictures.
/// </summary>
[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UrlBuilderService _urlBuilderService;

    public UserController(IUserService userService, UrlBuilderService urlBuilderService)
    {
        _userService = userService;
        _urlBuilderService = urlBuilderService;
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
    [Authorize]
    [ProducesResponseType(typeof(ProfileHeaderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileHeader(CancellationToken cancellationToken = default)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
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
    [Authorize]
    [ProducesResponseType(typeof(MyProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken = default)
    {
        string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();
        
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
    [ProducesResponseType(typeof(PublicProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserPublicProfile(string userId, CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetUserPublicProfileAsync(userId, cancellationToken);
        
        if (result.IsFaliure)
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
    [Authorize]
    [ProducesResponseType(typeof(MyProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        
        var result = await _userService.UpdateMyProfileAsync(currentUserId, request, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem();
    }
    
    /// <summary>
    /// Updates the authenticated user's profile picture.
    /// </summary>
    /// <remarks>
    /// Accepts image file upload in multipart/form-data format.
    /// Only standard image formats are accepted (jpg, jpeg, png, gif, webp).
    /// File size must not exceed the configured limit.
    ///
    /// Sample success response (200):
    ///
    ///     {
    ///       "profilePictureUrl": "https://api.example.com/images/profile/user-123.jpg"
    ///     }
    /// </remarks>
    /// <param name="profilePicture">The profile picture file to upload (must be an image file)</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns the URL of the newly uploaded profile picture</returns>
    /// <response code="200">Profile picture updated successfully - returns the new picture URL</response>
    /// <response code="400">Invalid file format or file size exceeds limit</response>
    /// <response code="401">Unauthorized - invalid or missing JWT token</response>
    /// <response code="404">User not found</response>
    /// <response code="415">Unsupported media type - file is not a valid image format</response>
    [HttpPost("me/profile-picture")]
    [Authorize]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    public async Task<IActionResult> UpdateProfilePicture([FromForm] ProfilePictureUpload profilePicture, CancellationToken cancellationToken = default)
    {
        string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();
        
        var result = await _userService.UpdateProfilePictureAsync(currentUserId,profilePicture.Image, cancellationToken);
        
        if (result.IsFaliure)
            return result.ToProblem();

        var profilePictureUrl = _urlBuilderService.ToAbsoluteUrl(result.Value);
        
        return Ok(new { profilePictureUrl });
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
    [Authorize]
    [ProducesResponseType(typeof(ProfilePictureResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfilePicture(CancellationToken cancellationToken = default)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _userService.GetProfilePictureAsync(userId, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}