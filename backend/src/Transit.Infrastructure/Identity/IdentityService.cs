using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Transit.Application.Abstractions.Identity;

namespace Transit.Infrastructure.Identity;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    IJwtTokenService jwtService,
    IConfiguration config) : IIdentityService
{
    public async Task<(bool success, string userId, IEnumerable<string> errors)> RejestrujUzytkownikaAsync(
        string email, string haslo, string rola)
    {
        var user = new ApplicationUser { UserName = email, Email = email };
        var result = await userManager.CreateAsync(user, haslo);
        if (!result.Succeeded)
            return (false, string.Empty, result.Errors.Select(e => e.Description));

        await userManager.AddToRoleAsync(user, rola);
        return (true, user.Id.ToString(), []);
    }

    public async Task<(bool success, string accessToken, string refreshToken, IEnumerable<string> errors)> ZalogujAsync(
        string email, string haslo)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null || !await userManager.CheckPasswordAsync(user, haslo))
            return (false, string.Empty, string.Empty, ["Nieprawidłowe dane logowania."]);

        return await WygenerujTokenyAsync(user);
    }

    public async Task<(bool success, string accessToken, string refreshToken, IEnumerable<string> errors)> OdswiezTokenAsync(
        string refreshToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u =>
            u.RefreshToken == refreshToken && u.RefreshTokenExpiry > DateTime.UtcNow);

        if (user is null)
            return (false, string.Empty, string.Empty, ["Nieprawidłowy lub wygasły refresh token."]);

        return await WygenerujTokenyAsync(user);
    }

    public async Task<bool> NadajRoleAsync(string userId, string rola)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return false;
        var result = await userManager.AddToRoleAsync(user, rola);
        return result.Succeeded;
    }

    public async Task<IEnumerable<string>> PobierzRoleUzytkownikaAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return [];
        return await userManager.GetRolesAsync(user);
    }

    private async Task<(bool, string, string, IEnumerable<string>)> WygenerujTokenyAsync(ApplicationUser user)
    {
        var role = await userManager.GetRolesAsync(user);
        var accessToken = jwtService.GenerujAccessToken(user.Id.ToString(), user.Email!, role);
        var refreshToken = jwtService.GenerujRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(int.Parse(config["Jwt:RefreshTokenExpiryDays"] ?? "30"));
        await userManager.UpdateAsync(user);

        return (true, accessToken, refreshToken, []);
    }
}
