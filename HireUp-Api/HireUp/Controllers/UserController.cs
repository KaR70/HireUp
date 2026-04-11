using HireUp.Abstraction;
using HireUp.DTOs.User;
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
    private readonly HireUp.Abstraction.IUserService _userService;
    private readonly UrlBuilderService _urlBuilderService;

    public UserController(HireUp.Abstraction.IUserService userService, UrlBuilderService urlBuilderService)
    {
        _userService = userService;
        _urlBuilderService = urlBuilderService;
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

    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

        var result = await _userService.UpdateMyProfileAsync(currentUserId, request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpGet("me/preferences")]
    [Authorize]
    public async Task<IActionResult> GetMyPreferences()
    {
        string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

        var preferences = await _userService.GetUserPreferencesAsync(currentUserId);
        return Ok(preferences);
    }

    [HttpPut("me/preferences")]
    [Authorize]
    public async Task<IActionResult> UpdateMyPreferences([FromBody] UpdateUserPreferencesRequest request)
    {
        string? currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

        var result = await _userService.UpdateUserPreferencesAsync(currentUserId, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}