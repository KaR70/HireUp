using HireUp.DTOs.User;
using HireUp.Abstraction;

namespace HireUp.Abstraction;

public interface IUserService
{
    Task<UserPreferencesResponse> GetUserPreferencesAsync(string userId);
    Task<Result> UpdateUserPreferencesAsync(string userId, UpdateUserPreferencesRequest request);
    Task<Result<MyProfileResponse>> GetMyProfileAsync(string currentUserId, CancellationToken cancellationToken = default);
    Task<Result<MyProfileResponse>> UpdateMyProfileAsync(string currentUserId, UpdateProfileRequest request, CancellationToken cancellationToken = default);
    Task<Result<PublicProfileResponse>> GetUserPublicProfileAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<string>> UpdateProfilePictureAsync(string currentUserId, IFormFile profilePicture, CancellationToken cancellationToken = default);
    Task<Result<ProfilePictureResponse>> GetProfilePictureAsync(string currentUserId, CancellationToken cancellationToken = default);
    Task<Result<ProfileHeaderResponse>> GetProfileHeaderAsync(string currentUserId, CancellationToken cancellationToken = default);
}