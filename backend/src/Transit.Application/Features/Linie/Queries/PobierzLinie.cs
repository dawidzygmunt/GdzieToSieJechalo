using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Models;

namespace Transit.Application.Features.Linie.Queries;

public record LiniaListDto(int Id, string NumerLinii, string TypLinii, string? Opis, bool Aktywna, int LiczbaWariantow);

public record PobierzLinieQuery(string? Szukaj = null, int Page = 1, int PageSize = 20)
    : IRequest<PaginatedList<LiniaListDto>>;

public class PobierzLinieHandler(IApplicationDbContext db) : IRequestHandler<PobierzLinieQuery, PaginatedList<LiniaListDto>>
{
    public Task<PaginatedList<LiniaListDto>> Handle(PobierzLinieQuery request, CancellationToken ct)
    {
        var query = db.Linie.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Szukaj))
            query = query.Where(l => l.NumerLinii.Contains(request.Szukaj));

        var projected = query.Select(l => new LiniaListDto(
            l.IdLinii, l.NumerLinii, l.TypLinii, l.Opis, l.Aktywna,
            l.Warianty.Count(w => w.Aktywny)));

        return PaginatedList<LiniaListDto>.CreateAsync(projected, request.Page, request.PageSize, ct);
    }
}
