namespace Transit.Application.Abstractions.Identity;

public interface IIdentityService
{
    Task<(bool success, string userId, IEnumerable<string> errors)> RejestrujUzytkownikaAsync(
        string email, string haslo, string rola);

    Task<(bool success, string accessToken, string refreshToken, IEnumerable<string> errors)> ZalogujAsync(
        string email, string haslo);

    Task<(bool success, string accessToken, string refreshToken, IEnumerable<string> errors)> OdswiezTokenAsync(
        string refreshToken);

    Task<bool> NadajRoleAsync(string userId, string rola);
    Task<IEnumerable<string>> PobierzRoleUzytkownikaAsync(string userId);
}
