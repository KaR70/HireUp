using HireUp.DTOs.User;

namespace HireUp.Services;

public interface IUserService
{
    Task<Result<MyProfileResponse>> GetMyProfileAsync(string currentUserId, CancellationToken cancellationToken = default);
    Task<Result<PublicProfileResponse>> GetUserPublicProfileAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> UpdateMyProfileAsync(string currentUserId, UpdateProfileRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> UpdateProfilePictureAsync(string currentUserId, IFormFile profilePicture, CancellationToken cancellationToken = default);
}
