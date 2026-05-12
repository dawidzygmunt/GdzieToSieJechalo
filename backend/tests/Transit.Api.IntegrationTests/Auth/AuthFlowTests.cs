using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Transit.Application.Features.Auth;
using Transit.Infrastructure.Identity;

namespace Transit.Api.IntegrationTests.Auth;

[Collection("Integration")]
public class AuthFlowTests(CustomWebApplicationFactory factory)
{
    private readonly HttpClient _client = factory.Client;

    [Fact]
    public async Task Login_prawidlowe_dane_zwraca_200_i_tokeny()
    {
        using var scope = factory.Services.CreateScope();
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        const string email = "logintest@test.pl";
        if (await userMgr.FindByEmailAsync(email) is null)
        {
            var user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
            await userMgr.CreateAsync(user, "Test123!");
            await userMgr.AddToRoleAsync(user, Roles.Admin);
        }

        var response = await _client.PostAsJsonAsync("/api/v1/auth/login",
            new { email, haslo = "Test123!" });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<AuthResponse>();
        body!.AccessToken.Should().NotBeNullOrEmpty();
        body.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_zle_haslo_zwraca_400()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login",
            new { email = "nieistnieje@test.pl", haslo = "ZleHaslo!" });
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
