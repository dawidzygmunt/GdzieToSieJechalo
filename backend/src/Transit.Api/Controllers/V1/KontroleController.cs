using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transit.Application.Features.Kontrole.Commands;
using Transit.Infrastructure.Identity;

namespace Transit.Api.Controllers.V1;

/// <summary>Kontrole biletów i mandaty.</summary>
[ApiVersion("1.0")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Kontroler}")]
public class KontroleController : ApiControllerBase
{
    /// <summary>Zapisz kontrolę w pojeździe.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Zapisz([FromBody] ZapiszKontroleCommand cmd, CancellationToken ct)
    {
        var id = await Sender.Send(cmd, ct);
        return Created($"/api/v1/kontrole/{id}", id);
    }

    /// <summary>Wystaw mandat w ramach kontroli.</summary>
    [HttpPost("{id}/mandaty")]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> WystawMandat(int id, [FromBody] WystawMandatRequest req, CancellationToken ct)
    {
        var mId = await Sender.Send(new WystawMandatCommand(id, req.Kwota, req.Powod, req.IdPasazera, req.NrDokumentu), ct);
        return Created($"/api/v1/kontrole/{id}/mandaty/{mId}", mId);
    }
}

public record WystawMandatRequest(decimal Kwota, string Powod, int? IdPasazera, string? NrDokumentu);
