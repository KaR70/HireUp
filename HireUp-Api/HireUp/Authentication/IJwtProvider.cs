using HireUp.Entities;

namespace HireUp.Authentication;

public interface IJwtProvider
{
    (string token, int ExpiresIn) GenerateToken(ApplicationUser user);
    string? ValidateToken(string token);
}
