using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transit.Application.Common.Models;
using Transit.Application.Features.Przystanki.Commands;
using Transit.Application.Features.Przystanki.Queries;
using Transit.Infrastructure.Identity;

namespace Transit.Api.Controllers.V1;

/// <summary>Zarządzanie przystankami.</summary>
[ApiVersion("1.0")]
public class PrzystankiController : ApiControllerBase
{
    /// <summary>Lista przystanków (publiczna).</summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedList<PrzystanekListDto>), 200)]
    public async Task<IActionResult> Lista([FromQuery] PobierzPrzystankiQuery q, CancellationToken ct) =>
        Ok(await Sender.Send(q, ct));

    /// <summary>Utwórz nowy przystanek.</summary>
    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Dyspozytor}")]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Utworz([FromBody] UtworzPrzystanekCommand cmd, CancellationToken ct)
    {
        var id = await Sender.Send(cmd, ct);
        return CreatedAtAction(nameof(Lista), new { id }, id);
    }
}
