using MediatR;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Models;

namespace Transit.Application.Features.Kierowcy.Queries;

public record KierowcaListDto(int Id, string Imie, string Nazwisko, string NrPracownika, DateOnly DataZatrudnienia, bool Aktywny);

public record PobierzKierowcowQuery(string? Szukaj = null, bool? Aktywny = null, int Page = 1, int PageSize = 20)
    : IRequest<PaginatedList<KierowcaListDto>>;

public class PobierzKierowcowHandler(IApplicationDbContext db) : IRequestHandler<PobierzKierowcowQuery, PaginatedList<KierowcaListDto>>
{
    public Task<PaginatedList<KierowcaListDto>> Handle(PobierzKierowcowQuery request, CancellationToken ct)
    {
        var query = db.Kierowcy.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Szukaj))
            query = query.Where(k => k.Nazwisko.Contains(request.Szukaj) || k.Imie.Contains(request.Szukaj) || k.NrPracownika.Contains(request.Szukaj));

        if (request.Aktywny.HasValue)
            query = query.Where(k => k.Aktywny == request.Aktywny.Value);

        var projected = query.Select(k => new KierowcaListDto(
            k.IdKierowcy, k.Imie, k.Nazwisko, k.NrPracownika, k.DataZatrudnienia, k.Aktywny));

        return PaginatedList<KierowcaListDto>.CreateAsync(projected, request.Page, request.PageSize, ct);
    }
}
