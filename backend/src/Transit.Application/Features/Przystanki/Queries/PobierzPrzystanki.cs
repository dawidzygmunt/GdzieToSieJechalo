using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Models;

namespace Transit.Application.Features.Przystanki.Queries;

public record PrzystanekListDto(int Id, string Nazwa, string Ulica, string Typ, bool Wiata, string Dzielnica, bool Aktywny);

public record PobierzPrzystankiQuery(int IdDzielnicy = 0, string? Szukaj = null, int Page = 1, int PageSize = 20)
    : IRequest<PaginatedList<PrzystanekListDto>>;

public class PobierzPrzystankiHandler(IApplicationDbContext db)
    : IRequestHandler<PobierzPrzystankiQuery, PaginatedList<PrzystanekListDto>>
{
    public Task<PaginatedList<PrzystanekListDto>> Handle(PobierzPrzystankiQuery request, CancellationToken ct)
    {
        var query = db.Przystanki
            .Include(p => p.Dzielnica)
            .AsQueryable();

        if (request.IdDzielnicy > 0)
            query = query.Where(p => p.IdDzielnicy == request.IdDzielnicy);

        if (!string.IsNullOrWhiteSpace(request.Szukaj))
            query = query.Where(p => p.Nazwa.Contains(request.Szukaj) || p.Ulica.Contains(request.Szukaj));

        var projected = query.Select(p => new PrzystanekListDto(
            p.IdPrzystanku, p.Nazwa, p.Ulica, p.Typ, p.Wiata, p.Dzielnica.Nazwa, p.Aktywny));

        return PaginatedList<PrzystanekListDto>.CreateAsync(projected, request.Page, request.PageSize, ct);
    }
}
