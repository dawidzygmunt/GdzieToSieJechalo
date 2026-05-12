using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Exceptions;

namespace Transit.Application.Features.Rozklady.Queries;

public record OdjazdZPrzystankuDto(
    string NumerLinii,
    string Kierunek,
    TimeOnly GodzinaOdjazdu,
    string NazwaPrzystankuDocelowego
);

public record PobierzOdjazdyZPrzystankuQuery(int IdPrzystanku, int OknoMin = 60)
    : IRequest<IReadOnlyList<OdjazdZPrzystankuDto>>;

public class PobierzOdjazdyZPrzystankuHandler(IApplicationDbContext db, Abstractions.Time.IDateTimeProvider time)
    : IRequestHandler<PobierzOdjazdyZPrzystankuQuery, IReadOnlyList<OdjazdZPrzystankuDto>>
{
    public async Task<IReadOnlyList<OdjazdZPrzystankuDto>> Handle(PobierzOdjazdyZPrzystankuQuery request, CancellationToken ct)
    {
        var przystanek = await db.Przystanki.FirstOrDefaultAsync(p => p.IdPrzystanku == request.IdPrzystanku && p.Aktywny, ct)
            ?? throw new NotFoundException("Przystanek", request.IdPrzystanku);

        var teraz = TimeOnly.FromDateTime(time.UtcNow);
        var dzisiaj = time.TodayUtc;
        var koniec = teraz.AddMinutes(request.OknoMin);

        var query = from pw in db.PrzystankiWariantu
                    join o in db.OdjazydPlanowe on pw.Id equals o.IdPrzystankuWariantu
                    join r in db.RozkladyJazdy on o.IdRozkladu equals r.IdRozkladu
                    join w in db.WariantyTras on pw.IdWariantu equals w.IdWariantu
                    join l in db.Linie on w.IdLinii equals l.IdLinii
                    where pw.IdPrzystanku == request.IdPrzystanku
                          && w.Aktywny && l.Aktywna
                          && r.Aktywny && r.DataWaznosciOd <= dzisiaj
                          && (r.DataWaznosciDo == null || r.DataWaznosciDo >= dzisiaj)
                          && o.PlanowaGodzinaOdjazdu >= teraz
                    orderby o.PlanowaGodzinaOdjazdu
                    select new OdjazdZPrzystankuDto(
                        l.NumerLinii,
                        w.Kierunek,
                        o.PlanowaGodzinaOdjazdu,
                        w.Kierunek
                    );

        return await query.Take(50).ToListAsync(ct);
    }
}
