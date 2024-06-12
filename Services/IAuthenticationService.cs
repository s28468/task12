using WebApplication1.Entities;

namespace WebApplication1.Services;

public interface IAuthenticationService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    Task<bool> ValidateExpiredAccessTokenAsync(string accessToken);
}