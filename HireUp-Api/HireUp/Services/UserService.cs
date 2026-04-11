using HireUp.Database.Interfaces;
using HireUp.Abstraction;
using HireUp.Database;
using HireUp.Entities;
using HireUp.DTOs.User;
using HireUp.DTOs.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace HireUp.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileService _fileService;
    private readonly ApplicationDbContext _context;
    private readonly UrlBuilderService _urlBuilderService;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(UserManager<ApplicationUser> userManager, IFileService fileService, UrlBuilderService urlBuilderService, IUnitOfWork unitOfWork)
    public UserService(
        UserManager<ApplicationUser> userManager,
        IFileService fileService,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _fileService = fileService;
        _context = context;
        _urlBuilderService = urlBuilderService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProfileHeaderResponse>> GetProfileHeaderAsync(string currentUserId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Include(u => u.UserDisabilityTypes).ThenInclude(udt => udt.DisabilityType)
            .Include(u => u.UserAccessibilityNeeds).ThenInclude(uan => uan.AccessibilityNeed)
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);
        
        if (user is null)
            return Result.Failure<ProfileHeaderResponse>(UserErrors.UserNotFound);

        var response = user.Adapt<ProfileHeaderResponse>();

        if (!string.IsNullOrEmpty(user.ProfilePicture))
            response.ProfilePictureUrl = _urlBuilderService.ToAbsoluteUrl(user.ProfilePicture);
        
        // TODO: Implement the real user verification logic. For now, all users are considered verified.
        // TODO: Implement the JobRole. for now its null always
        response.IsVerified = true;
        
        return Result.Success(response);
    }
    
    public async Task<Result<MyProfileResponse>> GetMyProfileAsync(string currentUserId, CancellationToken cancellationToken = default)
    {
        // TODO : Modify this to include the JobTitle
        var userProfile = await _userManager.Users
            .Where(u => u.Id == currentUserId)
            .ProjectToType<MyProfileResponse>()
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (userProfile is null)
            return Result.Failure<MyProfileResponse>(UserErrors.UserNotFound);

        return Result.Success(user.Adapt<MyProfileResponse>());
        
        if (!string.IsNullOrEmpty(userProfile.ProfilePictureUrl))
            userProfile.ProfilePictureUrl = _urlBuilderService.ToAbsoluteUrl(userProfile.ProfilePictureUrl);
        
        return Result.Success(userProfile);
    }

    public async Task<Result<PublicProfileResponse>> GetUserPublicProfileAsync(string userId, CancellationToken cancellationToken = default)
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

    public async Task<Result<MyProfileResponse>> UpdateMyProfileAsync(string currentUserId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
    public async Task<Result> UpdateMyProfileAsync(string currentUserId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = await _userManager.Users
            // TODO: Implement this after fixing the Git problem
            //.Include(u => u.JobTitle)
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
            return Result.Failure(UserErrors.UserNotFound);

        request.Adapt(currentUser); 

        // TODO: implement this after solving the git problem
        //currentUser.jobTitle = request.JobTitleId;

        foreach (var disabilityTypeId in request.DisabilityTypeIds)
        {
        request.DisabilityTypeIds?.ForEach(id => currentUser.UserDisabilityTypes.Add(new UserDisabilityType { UserId = currentUserId, DisabilityTypeId = id }));
        request.AccessibilityNeedsIds?.ForEach(id => currentUser.UserAccessibilityNeeds.Add(new UserAccessibilityNeed { UserId = currentUserId, AccessibilityNeedId = id }));

        var updatedResult = await _userManager.UpdateAsync(currentUser);
        return updatedResult.Succeeded ? Result.Success() : Result.Failure(UserErrors.UserUpdateFailed);
    }

    public async Task<UserPreferencesResponse> GetUserPreferencesAsync(string userId)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Include(u => u.UserJobTypePreferences).ThenInclude(p => p.JobType)
            .Include(u => u.UserOfficeTypePreferences).ThenInclude(p => p.OfficeType)
            .Include(u => u.UserLocationPreferences).ThenInclude(p => p.Location)
            .Include(u => u.UserJobCategoryPreferences).ThenInclude(p => p.JobCategory)
            .Include(u => u.UserJobRolePreferences).ThenInclude(p => p.JobRole)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null) return new UserPreferencesResponse();

        return new UserPreferencesResponse
        {
            JobTypes = user.UserJobTypePreferences.Select(p => new LookupDto { Id = p.JobTypeId, Name = p.JobType.Name }).ToList(),
            OfficeTypes = user.UserOfficeTypePreferences.Select(p => new LookupDto { Id = p.OfficeTypeId, Name = p.OfficeType.Name }).ToList(),
            Locations = user.UserLocationPreferences.Select(p => new LookupDto { Id = p.LocationId, Name = p.Location.Name }).ToList(),
            JobCategories = user.UserJobCategoryPreferences.Select(p => new LookupDto { Id = p.JobCategoryId, Name = p.JobCategory.Name }).ToList(),
            JobRoles = user.UserJobRolePreferences.Select(p => new LookupDto { Id = p.JobRoleId, Name = p.JobRole.Name }).ToList()
        };
    }

    public async Task<Result> UpdateUserPreferencesAsync(string userId, UpdateUserPreferencesRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = await _userManager.Users
                .Include(u => u.UserJobTypePreferences)
                .Include(u => u.UserOfficeTypePreferences)
                .Include(u => u.UserLocationPreferences)
                .Include(u => u.UserJobCategoryPreferences)
                .Include(u => u.UserJobRolePreferences)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null) return Result.Failure(UserErrors.UserNotFound);

            user.UserJobTypePreferences.Clear();
            user.UserOfficeTypePreferences.Clear();
            user.UserLocationPreferences.Clear();
            user.UserJobCategoryPreferences.Clear();
            user.UserJobRolePreferences.Clear();

            request.JobTypeIds?.ForEach(id => user.UserJobTypePreferences.Add(new UserJobTypePreference { JobTypeId = id }));
            request.OfficeTypeIds?.ForEach(id => user.UserOfficeTypePreferences.Add(new UserOfficeTypePreference { OfficeTypeId = id }));
            request.LocationIds?.ForEach(id => user.UserLocationPreferences.Add(new UserLocationPreference { LocationId = id }));
            request.JobCategoryIds?.ForEach(id => user.UserJobCategoryPreferences.Add(new UserJobCategoryPreference { JobCategoryId = id }));
            request.JobRoleIds?.ForEach(id => user.UserJobRolePreferences.Add(new UserJobRolePreference { JobRoleId = id }));

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return Result.Success();
        }
        catch
        {
            await transaction.RollbackAsync();
            return Result.Failure(UserErrors.UserUpdateFailed);
        }
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
    
        var response = currentUser.Adapt<MyProfileResponse>();
        
        if(!string.IsNullOrEmpty(response.ProfilePictureUrl))
            response.ProfilePictureUrl = _urlBuilderService.ToAbsoluteUrl(response.ProfilePictureUrl);
        
        return Result.Success(response);
    }

    public async Task<Result<string>> UpdateProfilePictureAsync(string currentUserId, IFormFile profilePicture, CancellationToken cancellationToken = default)
    {
        var currentUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);
        if (currentUser is null) return Result.Failure<string>(UserErrors.UserNotFound);

        var saveResult = await _fileService.SaveFileAsync(profilePicture, "images", cancellationToken);
        if (saveResult.IsFaliure) return Result.Failure<string>(FileErrors.ProfilePictureUploadFailed);

        currentUser.ProfilePicture = saveResult.Value;
        var updatedResult = await _userManager.UpdateAsync(currentUser);

        return updatedResult.Succeeded ? Result.Success(saveResult.Value) : Result.Failure<string>(UserErrors.UserUpdateFailed);
    }

    public async Task<Result<PublicProfileResponse>> GetUserPublicProfileAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user is null) return Result.Failure<PublicProfileResponse>(UserErrors.UserNotFound);
        return Result.Success(user.Adapt<PublicProfileResponse>());
    }

// public async Task<Result<string>> GetProfilePictureAsync(string currentUserId, CancellationToken cancellationToken = default)

    // public async Task<Result<string>> GetProfilePictureAsync(string currentUserId, CancellationToken cancellationToken = default)
    // {
    //     
    // }

}
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
    // {
    //     
    // }

}
