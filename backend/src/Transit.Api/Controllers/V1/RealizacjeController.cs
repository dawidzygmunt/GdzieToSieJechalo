using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transit.Application.Features.RealizacjeKursow.Commands;
using Transit.Infrastructure.Identity;

namespace Transit.Api.Controllers.V1;

/// <summary>Zarządzanie realizacjami kursów.</summary>
[ApiVersion("1.0")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Dyspozytor}")]
public class RealizacjeController : ApiControllerBase
{
    /// <summary>Utwórz nową realizację kursu.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Utworz([FromBody] UtworzRealizacjeCommand cmd, CancellationToken ct)
    {
        var id = await Sender.Send(cmd, ct);
        return Created($"/api/v1/realizacje/{id}", id);
    }

    /// <summary>Przypisz kierowcę i pojazd do realizacji.</summary>
    [HttpPost("{id}/przypisz")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Przypisz(int id, [FromBody] PrzypiszRequest req, CancellationToken ct)
    {
        await Sender.Send(new PrzypiszKierowcePojazdCommand(id, req.IdKierowcy, req.IdPojazdu), ct);
        return NoContent();
    }
}

public record PrzypiszRequest(int IdKierowcy, int IdPojazdu);
