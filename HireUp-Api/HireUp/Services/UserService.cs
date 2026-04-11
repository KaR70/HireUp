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

    public UserService(
        UserManager<ApplicationUser> userManager,
        IFileService fileService,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _fileService = fileService;
        _context = context;
    }

    public async Task<Result<MyProfileResponse>> GetMyProfileAsync(string currentUserId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Include(u => u.UserDisabilityTypes).ThenInclude(udt => udt.DisabilityType)
            .Include(u => u.UserAccessibilityNeeds).ThenInclude(uan => uan.AccessibilityNeed)
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

        if (user is null)
            return Result.Failure<MyProfileResponse>(UserErrors.UserNotFound);

        return Result.Success(user.Adapt<MyProfileResponse>());
    }

    public async Task<Result> UpdateMyProfileAsync(string currentUserId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = await _userManager.Users
            .Include(u => u.UserDisabilityTypes)
            .Include(u => u.UserAccessibilityNeeds)
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

        if (currentUser is null)
            return Result.Failure(UserErrors.UserNotFound);

        request.Adapt(currentUser); 

        currentUser.UserDisabilityTypes.Clear();
        currentUser.UserAccessibilityNeeds.Clear();

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
}