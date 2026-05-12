using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transit.Application.Common.Models;
using Transit.Application.Features.Linie.Commands;
using Transit.Application.Features.Linie.Queries;
using Transit.Infrastructure.Identity;

namespace Transit.Api.Controllers.V1;

/// <summary>Zarządzanie liniami komunikacyjnymi.</summary>
[ApiVersion("1.0")]
public class LinieController : ApiControllerBase
{
    /// <summary>Lista linii (publiczna).</summary>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedList<LiniaListDto>), 200)]
    public async Task<IActionResult> Lista([FromQuery] PobierzLinieQuery q, CancellationToken ct) =>
        Ok(await Sender.Send(q, ct));

    /// <summary>Utwórz nową linię.</summary>
    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Dyspozytor}")]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Utworz([FromBody] UtworzLinieCommand cmd, CancellationToken ct)
    {
        var id = await Sender.Send(cmd, ct);
        return CreatedAtAction(nameof(Lista), new { id }, id);
    }
}
