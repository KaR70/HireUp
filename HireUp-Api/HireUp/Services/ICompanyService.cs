using HireUp.DTOs.Company;

namespace HireUp.Services;

public interface ICompanyService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterProfileRequest request, CancellationToken cancellation = default);
}