using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Exceptions;

namespace Transit.Application.Features.Rozklady.Queries;

public record KursDto(int NrKursu, IReadOnlyList<StopDto> Przystanki);
public record StopDto(string NazwaPrzystanku, TimeOnly GodzinaOdjazdu);

public record PobierzRozkladLiniiQuery(int IdLinii, string KodTypuDnia) : IRequest<RozkladLiniiDto>;

public record RozkladLiniiDto(string NumerLinii, string KodTypuDnia, IReadOnlyList<WariantRozkladDto> Warianty);
public record WariantRozkladDto(string NazwaWariantu, string Kierunek, IReadOnlyList<KursDto> Kursy);

public class PobierzRozkladLiniiHandler(IApplicationDbContext db, Abstractions.Time.IDateTimeProvider time)
    : IRequestHandler<PobierzRozkladLiniiQuery, RozkladLiniiDto>
{
    public async Task<RozkladLiniiDto> Handle(PobierzRozkladLiniiQuery request, CancellationToken ct)
    {
        var linia = await db.Linie
            .Include(l => l.Warianty.Where(w => w.Aktywny))
            .FirstOrDefaultAsync(l => l.IdLinii == request.IdLinii && l.Aktywna, ct)
            ?? throw new NotFoundException("Linia", request.IdLinii);

        var typDnia = await db.TypyDni.FirstOrDefaultAsync(t => t.Kod == request.KodTypuDnia, ct)
            ?? throw new NotFoundException($"Typ dnia '{request.KodTypuDnia}' nie istnieje.");

        var dzisiaj = time.TodayUtc;
        var wariantyDto = new List<WariantRozkladDto>();

        foreach (var wariant in linia.Warianty)
        {
            var odjazdy = await (
                from r in db.RozkladyJazdy
                join o in db.OdjazydPlanowe on r.IdRozkladu equals o.IdRozkladu
                join pw in db.PrzystankiWariantu on o.IdPrzystankuWariantu equals pw.Id
                join p in db.Przystanki on pw.IdPrzystanku equals p.IdPrzystanku
                where r.IdWariantu == wariant.IdWariantu
                      && r.IdTypuDnia == typDnia.IdTypuDnia
                      && r.Aktywny
                      && r.DataWaznosciOd <= dzisiaj
                      && (r.DataWaznosciDo == null || r.DataWaznosciDo >= dzisiaj)
                orderby o.NrKursu, pw.Kolejnosc
                select new { o.NrKursu, p.Nazwa, o.PlanowaGodzinaOdjazdu }
            ).ToListAsync(ct);

            var kursy = odjazdy
                .GroupBy(o => o.NrKursu)
                .Select(g => new KursDto(g.Key, g.Select(x => new StopDto(x.Nazwa, x.PlanowaGodzinaOdjazdu)).ToList()))
                .ToList();

            wariantyDto.Add(new WariantRozkladDto(wariant.NazwaWariantu, wariant.Kierunek, kursy));
        }

        return new RozkladLiniiDto(linia.NumerLinii, request.KodTypuDnia, wariantyDto);
    }
}
