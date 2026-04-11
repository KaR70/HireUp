using HireUp.DTOs.User;
using HireUp.Abstraction; 

namespace HireUp.Abstraction;

public interface IUserService
{
    Task<UserPreferencesResponse> GetUserPreferencesAsync(string userId);
    Task<Result> UpdateUserPreferencesAsync(string userId, UpdateUserPreferencesRequest request);

    Task<Result<MyProfileResponse>> GetMyProfileAsync(string currentUserId, CancellationToken cancellationToken = default);
    Task<Result> UpdateMyProfileAsync(string currentUserId, UpdateProfileRequest request, CancellationToken cancellationToken = default);
}