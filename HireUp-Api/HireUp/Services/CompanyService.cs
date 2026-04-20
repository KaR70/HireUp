using HireUp.Database.Interfaces;
using HireUp.DTOs.Company;

namespace HireUp.Services;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly UserManager<ApplicationUser> _userManager;    
    
    public CompanyService(IUnitOfWork unitOfWork, IAuthService authService, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
        _userManager = userManager;
    }

    // public async Task<Result<ProfileResponse>> GetProfileAsync(string UserId, CancellationToken cancellationToken = default)
    // {
    //     
    // }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterProfileRequest request, CancellationToken cancellation = default)
    {
        var strategy = _unitOfWork.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellation);

            try
            {
                var registerRequest = new RegisterRequest
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Password = request.Password,
                };

                var registerResult = await _authService.RegisterAsync(registerRequest, cancellation);

                if (registerResult.IsFailure)
                    return Result.Failure<AuthResponse>(registerResult.Error);

                var userId = await _userManager.Users
                    .Where(x => x.Email == request.Email)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync(cancellation);

                if (userId == null)
                    return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

                if (request.LocationId.HasValue && !await _unitOfWork.Locations.ExistsAsync(request.LocationId.Value))
                    return Result.Failure<AuthResponse>(LocationErrors.NotFound);

                if (request.IndustryId.HasValue && !await _unitOfWork.Industry.ExistsAsync(request.IndustryId.Value))
                    return Result.Failure<AuthResponse>(IndustryErrors.NotFound);

                var company = request.Adapt<Company>();
                company.UserId = userId;

                await _unitOfWork.Companies.AddAsync(company);

                var isAdded = await _unitOfWork.SaveChangesAsync();

                if (isAdded == 0)
                    return Result.Failure<AuthResponse>(CompanyErrors.CreationFailed);
                
                await transaction.CommitAsync(cancellation);
            
                var authResponse = await _authService.GetTokenAsync(request.Email, request.Password, cancellation);
                return authResponse;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellation);
                
                var realErrorMessage = ex.InnerException?.Message ?? ex.Message;
                return Result.Failure<AuthResponse>(new Error("Debug.Crash", realErrorMessage));
                
                // return Result.Failure<AuthResponse>(CompanyErrors.CreationFailed);
            }
        });
    }
}