using HireUp.Authentication;
using HireUp.DTOs.Authentication;
using HireUp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HireUp.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly JwtOptions _jwtOptions;

    public AuthController(IAuthService authService, IOptions<JwtOptions> jwtOptions)
    {
        _authService = authService;
        _jwtOptions = jwtOptions.Value;
    }

    [HttpPost("")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsSuccess 
            ? Ok(authResult.Value) 
            : authResult.ToProblem();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsSuccess
            ? Ok(authResult.Value)
            : authResult.ToProblem();
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var Result = await _authService.RegisterAsync(request, cancellationToken);

        return Result.IsSuccess ? Ok() : Result.ToProblem();
    }
    
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken = default)
    {
        var Result = await _authService.ConfirmEmailAsync(request);

        return Result.IsSuccess ? Ok() : Result.ToProblem();
    }
    
    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResenConfirmationEmail([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken = default)
    {
        var Result = await _authService.ResendConfirmationEmailAsync(request);

        return Result.IsSuccess ? Ok() : Result.ToProblem();
    }
}
