using System.Security.Claims;

namespace Transit.Application.Abstractions.Identity;

public interface IJwtTokenService
{
    string GenerujAccessToken(string userId, string email, IEnumerable<string> role);
    string GenerujRefreshToken();
    ClaimsPrincipal? WalidujAccessToken(string token);
}
