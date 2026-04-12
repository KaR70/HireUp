using HireUp.Database.Interfaces;
using HireUp.DTOs.User;

namespace HireUp.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileService _fileService;
    private readonly UrlBuilderService _urlBuilderService;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(UserManager<ApplicationUser> userManager, IFileService fileService, UrlBuilderService urlBuilderService, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _fileService = fileService;
        _urlBuilderService = urlBuilderService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ProfileHeaderResponse>> GetProfileHeaderAsync(string currentUserId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .Include(u => u.JobRole)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        
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
    
    public async Task<Result<MyProfileResponse>> GetMyProfileAsync(string currentUserId, CancellationToken cancellationToken = default)
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
    
        var response = currentUser.Adapt<MyProfileResponse>();
        
        if(!string.IsNullOrEmpty(response.ProfilePictureUrl))
            response.ProfilePictureUrl = _urlBuilderService.ToAbsoluteUrl(response.ProfilePictureUrl);
        
        return Result.Success(response);
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

    public async Task<Result<ProfilePictureResponse>> GetProfilePictureAsync(string currentUserId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.
            AsNoTracking()
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

}
