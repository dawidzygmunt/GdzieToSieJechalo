using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transit.Application.Features.Polaczenia.Queries.WyszukajPolaczenia;

namespace Transit.Api.Controllers.V1;

/// <summary>Wyszukiwanie połączeń komunikacyjnych.</summary>
[ApiVersion("1.0")]
[AllowAnonymous]
public class PolaczeniaController : ApiControllerBase
{
    /// <summary>Wyszukuje połączenia z przystanku A do B w danym dniu i o podanej godzinie.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PolaczenieDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Wyszukaj(
        [FromQuery] int przystanekZ,
        [FromQuery] int przystanekDo,
        [FromQuery] DateOnly data,
        [FromQuery] TimeOnly czas,
        [FromQuery] int maxWynikow = 10,
        CancellationToken ct = default)
    {
        var result = await Sender.Send(
            new WyszukajPolaczeniaQuery(przystanekZ, przystanekDo, data, czas, maxWynikow), ct);
        return Ok(result);
    }
}
