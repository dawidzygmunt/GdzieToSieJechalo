using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transit.Application.Common.Models;
using Transit.Application.Features.Kierowcy.Commands;
using Transit.Application.Features.Kierowcy.Queries;
using Transit.Infrastructure.Identity;

namespace Transit.Api.Controllers.V1;

/// <summary>Zarządzanie kierowcami.</summary>
[ApiVersion("1.0")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Dyspozytor}")]
public class KierowcyController : ApiControllerBase
{
    /// <summary>Lista kierowców.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<KierowcaListDto>), 200)]
    public async Task<IActionResult> Lista([FromQuery] PobierzKierowcowQuery q, CancellationToken ct) =>
        Ok(await Sender.Send(q, ct));

    /// <summary>Utwórz nowego kierowcę.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Utworz([FromBody] UtworzKierowceCommand cmd, CancellationToken ct)
    {
        var id = await Sender.Send(cmd, ct);
        return CreatedAtAction(nameof(Lista), new { id }, id);
    }

    /// <summary>Nadaj uprawnienie kierowcy.</summary>
    [HttpPost("{id}/uprawnienia")]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> NadajUprawnienie(int id, [FromBody] NadajUprawnienieRequest req, CancellationToken ct)
    {
        var upId = await Sender.Send(new NadajUprawnienieCommand(id, req.IdUprawnienia, req.DataUzyskania, req.DataWaznosci), ct);
        return CreatedAtAction(nameof(Lista), new { id }, upId);
    }

    /// <summary>Dodaj badanie lekarskie kierowcy.</summary>
    [HttpPost("{id}/badania")]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> DodajBadanie(int id, [FromBody] DodajBadanieRequest req, CancellationToken ct)
    {
        var bId = await Sender.Send(new DodajBadanieLekarsieCommand(id, req.DataBadania, req.DataWaznosci, req.Wynik, req.Lekarz), ct);
        return CreatedAtAction(nameof(Lista), new { id }, bId);
    }
}

public record NadajUprawnienieRequest(int IdUprawnienia, DateOnly DataUzyskania, DateOnly DataWaznosci);
public record DodajBadanieRequest(DateOnly DataBadania, DateOnly DataWaznosci, string Wynik, string Lekarz);
