using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transit.Application.Features.Grafiki.Commands;
using Transit.Infrastructure.Identity;

namespace Transit.Api.Controllers.V1;

/// <summary>Zarządzanie grafikami pracy.</summary>
[ApiVersion("1.0")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Dyspozytor}")]
public class GrafikiController : ApiControllerBase
{
    /// <summary>Dodaj wpis do grafiku pracy.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Dodaj([FromBody] DodajGrafikCommand cmd, CancellationToken ct)
    {
        var id = await Sender.Send(cmd, ct);
        return Created($"/api/v1/grafiki/{id}", id);
    }
}
