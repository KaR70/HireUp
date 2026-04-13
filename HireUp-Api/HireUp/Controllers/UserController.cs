using HireUp.Database.Interfaces;
using HireUp.DTOs.User;
using HireUp.Mapping;
using HireUp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HireUp.Controllers;

[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly ISavedJobRepository _savedJobRepository;
    private readonly IUserService _userService;
    private readonly UrlBuilderService _urlBuilderService;

    public UserController(IUserService userService,
                          UrlBuilderService urlBuilderService,
                          ISavedJobRepository savedJobRepository)
    {
        _userService = userService;
        _urlBuilderService = urlBuilderService;
        _savedJobRepository = savedJobRepository;
    }

    [HttpGet("")]
    [Authorize]
    public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken = default)
    {
        string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();
        var result = await _userService.GetMyProfileAsync(currentUserId, cancellationToken);
        if (result.IsFaliure) return result.ToProblem();
        var profile = result.Value;
        profile.ProfilePictureUrl = _urlBuilderService.ToAbsoluteUrl(profile.ProfilePictureUrl);
        return Ok(profile);
    }

    [HttpGet("{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserPublicProfile(string userId, CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetUserPublicProfileAsync(userId, cancellationToken);
        if (result.IsFaliure) return result.ToProblem();
        var profile = result.Value;
        profile.ProfilePicture = _urlBuilderService.ToAbsoluteUrl(profile.ProfilePicture);
        return Ok(profile);
    }

    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();
        var result = await _userService.UpdateMyProfileAsync(currentUserId, request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("me/profile-picture")]
    [Authorize]
    public async Task<IActionResult> UpdateProfilePicture([FromForm] ProfilePictureUpload profilePicture, CancellationToken cancellationToken = default)
    {
        string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();
        var result = await _userService.UpdateProfilePictureAsync(currentUserId, profilePicture.Image, cancellationToken);
        if (result.IsFaliure) return result.ToProblem();
        var profilePictureUrl = _urlBuilderService.ToAbsoluteUrl(result.Value);
        return Ok(new { profilePictureUrl });
    }

    /// <summary>
    /// استرجاع الوظائف المحفوظة للمستخدم الحالي تلقائياً من التوكن
    /// </summary>
    [HttpGet("me/saved-jobs")]
    [Authorize] // لازم يكون عامل Login عشان نقدر نعرف الـ ID بتاعه
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
}