using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transit.Application.Common.Models;
using Transit.Application.Features.Pojazdy.Commands;
using Transit.Application.Features.Pojazdy.Queries;
using Transit.Application.Features.Przeglady.Commands;
using Transit.Infrastructure.Identity;

namespace Transit.Api.Controllers.V1;

/// <summary>Zarządzanie pojazdami i przeglądami.</summary>
[ApiVersion("1.0")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Dyspozytor}")]
public class PojazdyController : ApiControllerBase
{
    /// <summary>Lista pojazdów.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<PojazdListDto>), 200)]
    public async Task<IActionResult> Lista([FromQuery] PobierzPojazdyQuery q, CancellationToken ct) =>
        Ok(await Sender.Send(q, ct));

    /// <summary>Dodaj pojazd.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Utworz([FromBody] UtworzPojazdCommand cmd, CancellationToken ct)
    {
        var id = await Sender.Send(cmd, ct);
        return CreatedAtAction(nameof(Lista), new { id }, id);
    }

    /// <summary>Dodaj przegląd techniczny pojazdu.</summary>
    [HttpPost("{id}/przeglady")]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> DodajPrzeglad(int id, [FromBody] DodajPrzegladRequest req, CancellationToken ct)
    {
        var pId = await Sender.Send(new DodajPrzegladCommand(id, req.IdTypuPrzegladu, req.DataPrzegladu, req.Wynik, req.Warsztat, req.Uwagi), ct);
        return CreatedAtAction(nameof(Lista), new { id }, pId);
    }
}

public record DodajPrzegladRequest(int IdTypuPrzegladu, DateOnly DataPrzegladu, string Wynik, string Warsztat, string? Uwagi);
