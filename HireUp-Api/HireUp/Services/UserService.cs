using HireUp.DTOs.User;

namespace HireUp.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileService _fileService;

    public UserService(UserManager<ApplicationUser> userManager, IFileService fileService)
    {
        _userManager = userManager;
        _fileService = fileService;
    }

    public async Task<Result<MyProfileResponse>> GetMyProfileAsync(string currentUserId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Include(u => u.UserDisabilityTypes)
                .ThenInclude(udt => udt.DisabilityType)
            .Include(u => u.UserAccessibilityNeeds)
                .ThenInclude(uan => uan.AccessibilityNeed)
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<MyProfileResponse>(UserErrors.UserNotFound);
        }

        var response = user.Adapt<MyProfileResponse>();

        return Result.Success(response);
    }

    public async Task<Result<PublicProfileResponse>> GetUserPublicProfileAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            return Result.Failure<PublicProfileResponse>(UserErrors.UserNotFound);

        var response = user.Adapt<PublicProfileResponse>();

        return Result.Success(response);
    }

    public async Task<Result> UpdateMyProfileAsync(string currentUserId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = await _userManager.Users
            .Include(u => u.UserDisabilityTypes)
            .Include(u => u.UserAccessibilityNeeds)
            .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

        if (currentUser is null)
            return Result.Failure(UserErrors.UserNotFound);

        currentUser.FirstName = request.FirstName;
        currentUser.LastName = request.LastName;
        currentUser.Bio = request.Bio;
        currentUser.UserDisabilityTypes.Clear();
        currentUser.UserAccessibilityNeeds.Clear();

        foreach (var disabilityTypeId in request.DisabilityTypeIds)
        {
            currentUser.UserDisabilityTypes.Add(new UserDisabilityType
            {
                UserId = currentUserId,
                DisabilityTypeId = disabilityTypeId
            });
        }

        foreach (var accessibilityNeedIds in request.AccessibilityNeedsIds)
        {
            currentUser.UserAccessibilityNeeds.Add(new UserAccessibilityNeed
            {
                UserId = currentUserId,
                AccessibilityNeedId = accessibilityNeedIds
            });
        }

        var updatedResult = await _userManager.UpdateAsync(currentUser);

        if (!updatedResult.Succeeded)
        {
            return Result.Failure(UserErrors.UserUpdateFailed);
        }

        return Result.Success();
    }

    public async Task<Result<string>> UpdateProfilePictureAsync(string currentUserId, IFormFile profilePicture, CancellationToken cancellationToken = default)
    {
        var currentUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

        if (currentUser is null)
            return Result.Failure<string>(UserErrors.UserNotFound);
        
        var oldPictureRelativePath = currentUser.ProfilePicture;
        
        var saveResult = await _fileService.SaveFileAsync(profilePicture, "images", cancellationToken);
        
        if (saveResult.IsFaliure)
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

}
