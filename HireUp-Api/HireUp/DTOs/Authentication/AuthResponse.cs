namespace HireUp.DTOs.Authentication;

public class AuthResponse
{
    public string Id { get; set; }
    public string? Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Token { get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }

    public AuthResponse(string id, string? email, string firstName, string lastName, string token, int expiresIn, string refreshToken, DateTime refreshTokenExpiration)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Token = token;
        ExpiresIn = expiresIn;
        RefreshToken = refreshToken;
        RefreshTokenExpiration = refreshTokenExpiration;
    }
}
