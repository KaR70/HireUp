using HireUp.Database.Interfaces;
using HireUp.Abstraction;
using HireUp.Database;
using HireUp.Entities;
using HireUp.DTOs.User;
using HireUp.DTOs.Common;
using HireUp.DTOs.Disabled;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace HireUp.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileService _fileService;
    private readonly ApplicationDbContext _context;
    private readonly UrlBuilderService _urlBuilderService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public UserService(
        UserManager<ApplicationUser> userManager,
        IFileService fileService,
        UrlBuilderService urlBuilderService,
        IUnitOfWork unitOfWork,
        IAuthService authService,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _fileService = fileService;
        _urlBuilderService = urlBuilderService;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _context = context;
    }

    public async Task<Result<ProfileHeaderResponse>> GetProfileHeaderAsync(string currentUserId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .Include(u => u.JobRole)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

        if (user is null)
            return Result.Failure<ProfileHeaderResponse>(UserErrors.UserNotFound);

        var response = user.Adapt<ProfileHeaderResponse>();
        if (!string.IsNullOrEmpty(user.ProfilePicture))
            response.ProfilePictureUrl = _urlBuilderService.ToAbsoluteUrl(user.ProfilePicture);

        // TODO: Implement the real user verification logic. For now, all users are considered verified.
        response.JobRole = user.JobRole?.Name;
        response.IsVerified = true;

        return Result.Success(response);
    }

    public async Task<Result<MyProfileResponse>> GetMyProfileAsync(string currentUserId,
        CancellationToken cancellationToken = default)
    {
        var userProfile = await _userManager.Users
            .Where(u => u.Id == currentUserId)
            .ProjectToType<MyProfileResponse>()
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (userProfile is null)
            return Result.Failure<MyProfileResponse>(UserErrors.UserNotFound);

        if (!string.IsNullOrEmpty(userProfile.ProfilePictureUrl))
            userProfile.ProfilePictureUrl = _urlBuilderService.ToAbsoluteUrl(userProfile.ProfilePictureUrl);

        return Result.Success(userProfile);
    }

    public async Task<Result<PublicProfileResponse>> GetUserPublicProfileAsync(string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == userId)
            .AsNoTracking()
            .ProjectToType<PublicProfileResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result.Failure<PublicProfileResponse>(UserErrors.UserNotFound);

        var Rating = await _unitOfWork.Reviews.GetAverageRatingForUserAsync(userId, cancellationToken);
        var followerCount = await _unitOfWork.Follows.CountAsync(f => f.FollowingId == userId, cancellationToken);

        // TODO: implement Project Logic
        user.ProjectCount = 0;
        user.Rating = Rating ?? 0;
        user.FollowersCount = followerCount;

        return Result.Success(user);
    }

    public async Task<Result> UpdateMyProfileAsync(string currentUserId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = await _userManager.Users
            .Include(u => u.JobRole)
            .Include(u => u.Location)
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

        if (currentUser is null)
            return Result.Failure<MyProfileResponse>(UserErrors.UserNotFound);

        if (string.IsNullOrEmpty(request.City) || string.IsNullOrEmpty(request.Country))
        {
            currentUser.Location = null;
        }
        else
        {
            var Location = await _unitOfWork.Locations
                .FirstOrDefaultAsync(l => l.City == request.City && l.Country == request.Country, cancellationToken);

            if (Location is null)
            {
                Location = new Location { City = request.City, Country = request.Country };
                _unitOfWork.Locations.AddAsync(Location);
            }

            currentUser.Location = Location;
        }

        currentUser.FirstName = request.FirstName;
        currentUser.LastName = request.LastName;
        currentUser.Birthday = request.Birthday;
        currentUser.PhoneNumber = request.PhoneNumber;
        currentUser.Header = request.Header;
        currentUser.Bio = request.Bio;
        currentUser.JobRoleId = request.JobRoleId;

        if (string.IsNullOrEmpty(request.Gender))
        {
            currentUser.Gender = null;
        }
        else if (Enum.TryParse<Gender>(request.Gender, true, out var genderEnum))
        {
            currentUser.Gender = genderEnum;
        }
        else
        {
            currentUser.Gender = null;
        }

        var updatedResult = await _userManager.UpdateAsync(currentUser);
        if (!updatedResult.Succeeded)
            return Result.Failure<MyProfileResponse>(UserErrors.UserUpdateFailed);

        //var response = currentUser.Adapt<MyProfileResponse>();

        // if (!string.IsNullOrEmpty(response.ProfilePictureUrl))
        //     response.ProfilePictureUrl = _urlBuilderService.ToAbsoluteUrl(response.ProfilePictureUrl);

        
        
        return Result.Success();
    }

    public async Task<UserPreferencesResponse> GetUserPreferencesAsync(string userId)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Include(u => u.UserJobTypePreferences).ThenInclude(p => p.JobType)
            .Include(u => u.UserOfficeTypePreferences).ThenInclude(p => p.OfficeType)
            .Include(u => u.UserLocationPreferences).ThenInclude(p => p.Location)
            .Include(u => u.UserJobCategoryPreferences).ThenInclude(p => p.JobCategory)
            .Include(u => u.JobRole)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null) return new UserPreferencesResponse();

        return new UserPreferencesResponse
        {
            JobTypes = user.UserJobTypePreferences
                .Select(p => new LookupDto { Id = p.JobTypeId, Name = p.JobType.Name }).ToList(),
            OfficeTypes = user.UserOfficeTypePreferences
                .Select(p => new LookupDto { Id = p.OfficeTypeId, Name = p.OfficeType.Name }).ToList(),
            Locations = user.UserLocationPreferences
                .Select(p => new LookupDto { Id = p.LocationId, Name = p.Location.City }).ToList(),
            //TODO: Decide wether to keep this or not
            // JobCategories = user.UserJobCategoryPreferences
            //     .Select(p => new LookupDto { Id = p.JobCategoryId, Name = p.JobCategory.Name }).ToList(),
            JobRole = user.JobRole != null 
                ? new LookupDto { Id = user.JobRole.Id, Name = user.JobRole.Name } 
                : null
        };
    }

    public async Task<Result> UpdateUserPreferencesAsync(string userId, UpdateUserPreferencesRequest request)
    {
        //using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = await _userManager.Users
                .Include(u => u.UserJobTypePreferences)
                .Include(u => u.UserOfficeTypePreferences)
                .Include(u => u.UserLocationPreferences)
                .Include(u => u.UserJobCategoryPreferences)
                .Include(u => u.JobRole)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null) return Result.Failure(UserErrors.UserNotFound);

            user.UserJobTypePreferences.Clear();
            user.UserOfficeTypePreferences.Clear();
            user.UserLocationPreferences.Clear();
            user.UserJobCategoryPreferences.Clear();

            request.JobTypeIds?.ForEach(id =>
                user.UserJobTypePreferences.Add(new UserJobTypePreference { JobTypeId = id }));
            request.OfficeTypeIds?.ForEach(id =>
                user.UserOfficeTypePreferences.Add(new UserOfficeTypePreference { OfficeTypeId = id }));
            request.LocationIds?.ForEach(id =>
                user.UserLocationPreferences.Add(new UserLocationPreference { LocationId = id }));
            
            //TODO: Decide wether to keep this or not
            // request.JobCategoryIds?.ForEach(id =>
            //     user.UserJobCategoryPreferences.Add(new UserJobCategoryPreference { JobCategoryId = id }));
            
            user.JobRoleId = request.JobRoleId;

            await _context.SaveChangesAsync();
            //await transaction.CommitAsync();
            return Result.Success();
        }
        catch
        {
            //await transaction.RollbackAsync();
            return Result.Failure(UserErrors.UserUpdateFailed);
        }
    }

    public async Task<Result<string>> UpdateProfilePictureAsync(string currentUserId, IFormFile profilePicture, CancellationToken cancellationToken = default)
    {
        var currentUser =
            await _userManager.Users.FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

        if (currentUser is null)
            return Result.Failure<string>(UserErrors.UserNotFound);

        var oldPictureRelativePath = currentUser.ProfilePicture;

        var saveResult = await _fileService.SaveFileAsync(profilePicture, "images", cancellationToken);

        if (saveResult.IsFailure)
            return Result.Failure<string>(FileErrors.ProfilePictureUploadFailed);

        var newPictureRelativePath = saveResult.Value;
        currentUser.ProfilePicture = newPictureRelativePath;

        var updatedResult = await _userManager.UpdateAsync(currentUser);

        if (!updatedResult.Succeeded)
        {
            _fileService.DeleteFile("images", Path.GetFileName(newPictureRelativePath)!);
            return Result.Failure<string>(UserErrors.UserUpdateFailed);
        }

        if (!string.IsNullOrEmpty(oldPictureRelativePath))
            _fileService.DeleteFile("images", Path.GetFileName(oldPictureRelativePath)!);

        return Result.Success(newPictureRelativePath);
    }

    public async Task<Result<ProfilePictureResponse>> GetProfilePictureAsync(string currentUserId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

        if (user is null)
            return Result.Failure<ProfilePictureResponse>(UserErrors.UserNotFound);

        var relativePath = user.ProfilePicture;
        string? absoluteUrl = null;

        if (!string.IsNullOrEmpty(relativePath))
            absoluteUrl = _urlBuilderService.ToAbsoluteUrl(relativePath);

        var result = new ProfilePictureResponse()
        {
            ProfilePictureUrl = absoluteUrl
        };

        return Result.Success(result);
    }

    public async Task<Result<AuthResponse>> DisabledRegister(DisabledRegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var strategy = _unitOfWork.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var registerRequest = new RegisterRequest
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Password = request.Password
                };
                
                var registerResult = await _authService.RegisterAsync(registerRequest, DefaultRoles.DisabledFreelancer, cancellationToken);

                if (registerResult.IsFailure)
                    return Result.Failure<AuthResponse>(registerResult.Error);
                
                if (!await _unitOfWork.Locations.ExistsAsync(request.LocationId))
                    return Result.Failure<AuthResponse>(LocationErrors.NotFound);
                
                var user = await _userManager.FindByEmailAsync(request.Email);
                
                if (user == null)
                    return Result.Failure<AuthResponse>(UserErrors.UserNotFound);
                
                user.Birthday = request.Birthday;
                user.PhoneNumber = request.PhoneNumber;
                user.LocationId = request.LocationId;
                
                if (string.IsNullOrEmpty(request.Gender))
                {
                    user.Gender = null;
                }
                else if (Enum.TryParse<Gender>(request.Gender, true, out var genderEnum))
                {
                    user.Gender = genderEnum;
                }
                else
                {
                    user.Gender = null;
                }
                
                var updatedResult = await _userManager.UpdateAsync(user);
                if (!updatedResult.Succeeded)
                    return Result.Failure<AuthResponse>(UserErrors.UserUpdateFailed);
                
                await transaction.CommitAsync(cancellationToken);
            
                var authResponse = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
                return authResponse;
                
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                var realErrorMessage = ex.InnerException?.Message ?? ex.Message;
                return Result.Failure<AuthResponse>(new Error("Debug.Crash", realErrorMessage));
            }
        });
    }
}