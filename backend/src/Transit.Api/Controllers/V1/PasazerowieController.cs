using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transit.Application.Features.Bilety.Commands;
using Transit.Application.Features.Pasazerowie.Commands;
using Transit.Infrastructure.Identity;

namespace Transit.Api.Controllers.V1;

/// <summary>Zarządzanie pasażerami i biletami.</summary>
[ApiVersion("1.0")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Dyspozytor}")]
public class PasazerowieController : ApiControllerBase
{
    /// <summary>Utwórz pasażera.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Utworz([FromBody] UtworzPasazeraCommand cmd, CancellationToken ct)
    {
        var id = await Sender.Send(cmd, ct);
        return Created($"/api/v1/pasazerowie/{id}", id);
    }

    /// <summary>Wystaw bilet okresowy.</summary>
    [HttpPost("{id}/bilety")]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> WystawnBilet(int id, [FromBody] WystawnBiletRequest req, CancellationToken ct)
    {
        var bId = await Sender.Send(new WystawnBiletCommand(id, req.IdKategorii, req.DataOd, req.DataDo, req.Cena), ct);
        return Created($"/api/v1/pasazerowie/{id}/bilety/{bId}", bId);
    }
}

public record WystawnBiletRequest(int IdKategorii, DateOnly DataOd, DateOnly DataDo, decimal Cena);
