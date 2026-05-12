using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transit.Application.Features.Rozklady.Queries;

namespace Transit.Api.Controllers.V1;

/// <summary>Rozkłady jazdy.</summary>
[ApiVersion("1.0")]
[AllowAnonymous]
public class RozkladyController : ApiControllerBase
{
    /// <summary>Tablica odjazdów z przystanku w oknie czasowym (minuty).</summary>
    [HttpGet("/api/v{version:apiVersion}/przystanki/{id}/odjazdy")]
    [ProducesResponseType(typeof(IReadOnlyList<OdjazdZPrzystankuDto>), 200)]
    public async Task<IActionResult> OdjazdyZPrzystanku(int id, [FromQuery] int okno = 60, CancellationToken ct = default)
    {
        var result = await Sender.Send(new PobierzOdjazdyZPrzystankuQuery(id, okno), ct);
        return Ok(result);
    }

    /// <summary>Rozkład linii dla podanego kodu typu dnia (ROB/SOB/SWI).</summary>
    [HttpGet("/api/v{version:apiVersion}/linie/{id}/rozklad")]
    [ProducesResponseType(typeof(RozkladLiniiDto), 200)]
    public async Task<IActionResult> RozkladLinii(int id, [FromQuery] string typDnia = "ROB", CancellationToken ct = default)
    {
        var result = await Sender.Send(new PobierzRozkladLiniiQuery(id, typDnia), ct);
        return Ok(result);
    }
}
