using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transit.Application.Features.Auth;

namespace Transit.Api.Controllers.V1;

/// <summary>Uwierzytelnianie i autoryzacja.</summary>
[ApiVersion("1.0")]
[AllowAnonymous]
public class AuthController : ApiControllerBase
{
    /// <summary>Logowanie — zwraca access token i refresh token.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login([FromBody] LoginCommand cmd, CancellationToken ct)
    {
        var result = await Sender.Send(cmd, ct);
        return Ok(result);
    }

    /// <summary>Odświeżenie access tokenu za pomocą refresh tokenu.</summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand cmd, CancellationToken ct)
    {
        var result = await Sender.Send(cmd, ct);
        return Ok(result);
    }
}
