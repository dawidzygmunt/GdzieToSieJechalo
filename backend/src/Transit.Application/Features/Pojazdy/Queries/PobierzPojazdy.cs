using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Models;

namespace Transit.Application.Features.Pojazdy.Queries;

public record PojazdListDto(int Id, string NumerBoczny, string Vin, int RokProdukcji, string Model, string Producent, bool Aktywny);

public record PobierzPojazdyQuery(bool? Aktywny = null, int Page = 1, int PageSize = 20) : IRequest<PaginatedList<PojazdListDto>>;

public class PobierzPojazdyHandler(IApplicationDbContext db) : IRequestHandler<PobierzPojazdyQuery, PaginatedList<PojazdListDto>>
{
    public Task<PaginatedList<PojazdListDto>> Handle(PobierzPojazdyQuery request, CancellationToken ct)
    {
        var query = db.Pojazdy.Include(p => p.Model).ThenInclude(m => m.Producent).AsQueryable();

        if (request.Aktywny.HasValue)
            query = query.Where(p => p.Aktywny == request.Aktywny.Value);

        var projected = query.Select(p => new PojazdListDto(
            p.IdPojazdu, p.NumerBoczny, p.Vin, p.RokProdukcji,
            p.Model.NazwaModelu, p.Model.Producent.Nazwa, p.Aktywny));

        return PaginatedList<PojazdListDto>.CreateAsync(projected, request.Page, request.PageSize, ct);
    }
}
