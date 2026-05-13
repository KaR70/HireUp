using HireUp.Authentication;
using HireUp.DTOs.Authentication;
using HireUp.DTOs.Company;
using HireUp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HireUp.Controllers;

/// <summary>
/// Provides endpoints for user authentication including login, registration, email confirmation, and password reset functionality.
/// </summary>
[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly JwtOptions _jwtOptions;
    private readonly ICompanyService _companyService;

    public AuthController(IAuthService authService, IOptions<JwtOptions> jwtOptions, ICompanyService companyService)
    {
        _authService = authService;
        _companyService = companyService;
        _jwtOptions = jwtOptions.Value;
    }

    /// <summary>
    /// Authenticates a user and returns JWT access token and refresh token.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /auth
    ///     {
    ///       "email": "user@example.com",
    ///       "password": "SecurePassword123!"
    ///     }
    ///
    /// Sample success response (200):
    ///
    ///     {
    ///       "id": "user-id",
    ///       "email": "user@example.com",
    ///       "firstName": "John",
    ///       "lastName": "Doe",
    ///       "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    ///       "expiresIn": 3600,
    ///       "refreshToken": "refresh-token-value",
    ///       "refreshTokenExpiration": "2024-12-31T23:59:59Z"
    ///     }
    ///
    /// Sample error response (401 - Invalid credentials):
    ///
    ///     {
    ///       "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
    ///       "title": "Unauthorized",
    ///       "status": 401,
    ///       "detail": "Invalid email/password",
    ///       "instance": null,
    ///       "error": ["User.InvalidCredentials", "Invalid email/password"]
    ///     }
    /// </remarks>
    /// <param name="request">The login credentials (email and password)</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns an AuthResponse containing JWT token, refresh token, and user information</returns>
    /// <response code="200">Login successful - returns auth tokens and user information</response>
    /// <response code="400">Invalid request format or validation failed</response>
    /// <response code="401">Invalid credentials (email/password mismatch) or email not confirmed</response>
    [HttpPost("")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return authResult.IsSuccess 
            ? Ok(authResult.Value) 
            : authResult.ToProblem();
    }

    /// <summary>
    /// Refreshes an expired JWT access token using a valid refresh token.
    /// </summary>
    /// <remarks>
    /// Sample error response (401):
    ///
    ///     {
    ///       "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
    ///       "title": "Unauthorized",
    ///       "status": 401,
    ///       "detail": "Invalid refresh token",
    ///       "error": ["User.InvalidRefreshToken", "Invalid refresh token"]
    ///     }
    /// </remarks>
    /// <param name="request">Contains the expired token and valid refresh token</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns a new AuthResponse with fresh tokens</returns>
    /// <response code="200">Token refresh successful - returns new access and refresh tokens</response>
    /// <response code="400">Invalid request format</response>
    /// <response code="401">Invalid or expired refresh token</response>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsSuccess
            ? Ok(authResult.Value)
            : authResult.ToProblem();
    }

    /// <summary>
    /// Revokes a refresh token, preventing it from being used for future token refreshes.
    /// </summary>
    /// <remarks>
    /// This endpoint allows users to logout by invalidating their refresh token.
    /// After revocation, the refresh token cannot be used to obtain new access tokens.
    /// </remarks>
    /// <param name="request">Contains the access token and refresh token to revoke</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns 200 OK if revocation was successful</returns>
    /// <response code="200">Token revocation successful</response>
    /// <response code="400">Invalid request format</response>
    /// <response code="401">Invalid or already revoked token</response>
    [HttpPost("revoke-refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RevokeRefresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
    
    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <remarks>
    /// Sample error response (409 - Email already exists):
    ///
    ///     {
    ///       "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
    ///       "title": "Conflict",
    ///       "status": 409,
    ///       "detail": "Another user with the same email is already exists",
    ///       "error": ["User.DuplicatedEmail", "Another user with the same email is already exists"]
    ///     }
    ///
    /// After successful registration, a confirmation email is sent. User must confirm email before logging in.
    /// </remarks>
    /// <param name="request">Contains user registration details (email, password, firstName, lastName)</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns 200 OK if registration was successful. User must confirm email before login.</returns>
    /// <response code="200">Registration successful - confirmation email sent to user's email address</response>
    /// <response code="400">Invalid request format or validation failed</response>
    /// <response code="409">Email already registered by another user</response>
    /// <response code="422">Validation error (weak password, invalid format, etc.)</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var Result = await _authService.FreelancerRegisterAsync(request, cancellationToken);

        return Result.IsSuccess ? Ok() : Result.ToProblem();
    }

    /// <summary>
    /// Registers a new company account with comprehensive profile information.
    /// </summary>
    /// <remarks>
    /// Creates a new company profile with user credentials and company details.
    /// The user account is automatically created with the provided email and password.
    /// After successful registration, a confirmation email is sent to the company email.
    ///
    /// Sample request:
    ///
    ///     {
    ///       "firstName": "Jane",
    ///       "lastName": "Smith",
    ///       "email": "jane.smith@company.com",
    ///       "password": "SecurePassword123!",
    ///       "name": "TechCorp Inc",
    ///       "description": "A leading software development company specializing in enterprise solutions",
    ///       "industryId": 1,
    ///       "locationId": 2,
    ///       "website": "https://techcorp.com",
    ///       "linkedin": "https://linkedin.com/company/techcorp",
    ///       "foundedYear": 2010
    ///     }
    /// </remarks>
    /// <param name="request">Company registration details including user credentials and company profile</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns the created company profile information</returns>
    /// <response code="200">Company registration successful - confirmation email sent</response>
    /// <response code="400">Invalid request format or validation failed</response>
    /// <response code="409">Email already registered by another user</response>
    /// <response code="422">Validation error (weak password, invalid data, etc.)</response>
    [HttpPost("register/company")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RegisterCompany([FromBody] RegisterProfileRequest request, CancellationToken cancellationToken)
    {
        var result = await _companyService.RegisterAsync(request, cancellationToken);
        
        return result.IsSuccess 
            ? Ok(result.Value) 
            : result.ToProblem();
    }
    
    /// <summary>
    /// Confirms a user's email address using a confirmation code.
    /// </summary>
    /// <remarks>
    /// The confirmation code is sent to the user's email address during registration.
    /// This must be completed before the user can log in.
    ///
    /// Sample error response (401 - Invalid code):
    ///
    ///     {
    ///       "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
    ///       "title": "Unauthorized",
    ///       "status": 401,
    ///       "detail": "Invalid Code",
    ///       "error": ["User.InvalidCode", "Invalid Code"]
    ///     }
    /// </remarks>
    /// <param name="request">Contains userId and confirmation code received via email</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns 200 OK if email confirmation was successful</returns>
    /// <response code="200">Email confirmed successfully - user can now log in</response>
    /// <response code="400">Invalid userId, confirmation already completed, or request format error</response>
    /// <response code="401">Invalid or expired confirmation code</response>
    /// <response code="404">User not found</response>
    [HttpPost("confirm-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken = default)
    {
        var Result = await _authService.ConfirmEmailAsync(request);

        return Result.IsSuccess ? Ok() : Result.ToProblem();
    }
    
    /// <summary>
    /// Resends the email confirmation code to the user's registered email address.
    /// </summary>
    /// <remarks>
    /// Use this endpoint if the user did not receive the initial confirmation email
    /// or if the code has expired.
    /// </remarks>
    /// <param name="request">Contains the user's email address</param>
    /// <param name="cancellationToken">Cancellation token for the async operation</param>
    /// <returns>Returns 200 OK if resend was successful</returns>
    /// <response code="200">Confirmation email resent successfully</response>
    /// <response code="400">Invalid email, user not found, or request format error</response>
    /// <response code="404">User not found with specified email</response>
    [HttpPost("resend-confirmation-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResenConfirmationEmail([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken = default)
    {
        var Result = await _authService.ResendConfirmationEmailAsync(request);

        return Result.IsSuccess ? Ok() : Result.ToProblem();
    }
    
    /// <summary>
    /// Initiates the password reset process by sending a reset code to the user's email.
    /// </summary>
    /// <remarks>
    /// This endpoint starts the password reset workflow by sending a temporary reset code
    /// to the user's registered email address. The user can then use this code with the
    /// /auth/reset-password endpoint to set a new password.
    /// </remarks>
    /// <param name="request">Contains the user's email address</param>
    /// <returns>Returns 200 OK if reset code was sent successfully</returns>
    /// <response code="200">Password reset code sent to user's email address</response>
    /// <response code="400">Invalid email or request format error</response>
    /// <response code="404">User not found with specified email</response>
    [HttpPost("forget-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var Result = await _authService.SendResetPasswordCodeAsync(request.Email);

        return Result.IsSuccess ? Ok() : Result.ToProblem();
    }
    
    /// <summary>
    /// Resets the user's password using the reset code sent to their email.
    /// </summary>
    /// <remarks>
    /// Sample error response (400 - Invalid reset code):
    ///
    ///     {
    ///       "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
    ///       "title": "Bad Request",
    ///       "status": 400,
    ///       "detail": "The code is invalid or has expired. Please try again or request a new one.",
    ///       "error": ["User.InvalidResetCode", "The code is invalid or has expired. Please try again or request a new one."]
    ///     }
    ///
    /// Password must meet complexity requirements (minimum 8 characters).
    /// </remarks>
    /// <param name="request">Contains email, reset code, and new password</param>
    /// <returns>Returns 200 OK if password reset was successful</returns>
    /// <response code="200">Password reset successfully - user can now log in with new password</response>
    /// <response code="400">Invalid reset code, expired code, invalid email, or weak password</response>
    /// <response code="404">User not found with specified email</response>
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
