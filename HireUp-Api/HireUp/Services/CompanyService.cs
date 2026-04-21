using HireUp.Database.Interfaces;
using HireUp.DTOs.Company;

namespace HireUp.Services;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UrlBuilderService _urlBuilderService;
    
    public CompanyService(IUnitOfWork unitOfWork, IAuthService authService, UserManager<ApplicationUser> userManager, UrlBuilderService urlBuilderService)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
        _userManager = userManager;
        _urlBuilderService = urlBuilderService;
    }

    public async Task<Result<CompanyHomeResponse>> GetHomeAsync(string UserId,
        CancellationToken cancellationToken = default)
    {
        var isExist = await _userManager.Users.AnyAsync(x => x.Id == UserId, cancellationToken);

        if (!isExist)
            return Result.Failure<CompanyHomeResponse>(UserErrors.UserNotFound);

        var company = await _unitOfWork.Companies.GetByUserIdAsync(UserId, cancellationToken);

        if (company == null)
            return Result.Failure<CompanyHomeResponse>(CompanyErrors.NotFound);

        var activeJobsCount = await _unitOfWork.Companies.CountActiveJobsAsync(UserId, cancellationToken);

        var totalApplicantsCount = await _unitOfWork.Companies.CountTotalApplicantsAsync(UserId, cancellationToken);

        var recentApplicants = await _unitOfWork.Applications.GetRecentApplicantsAsync(UserId, 4, cancellationToken);

        var rawDtos = recentApplicants.Adapt<List<RecentApplicantDto>>();
        
        var recentApplicantDtos = rawDtos.Select(x => 
            x with
            {
                ApplicantProfilePictureUrl = string.IsNullOrWhiteSpace(x.ApplicantProfilePictureUrl)
                    ? null 
                    : _urlBuilderService.ToAbsoluteUrl(x.ApplicantProfilePictureUrl)
            }
        ).ToList();

        var response = new CompanyHomeResponse(

            company.Name,
            string.IsNullOrWhiteSpace(company.Logo) ? null : _urlBuilderService.ToAbsoluteUrl(company.Logo),
            activeJobsCount,
            totalApplicantsCount,
            recentApplicantDtos
        );

        return Result.Success(response);
    }

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