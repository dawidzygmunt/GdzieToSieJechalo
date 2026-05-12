using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Routing;
using Transit.Application.Features.Polaczenia.Queries.WyszukajPolaczenia;
using Transit.Infrastructure.Persistence;

namespace Transit.Infrastructure.Routing;

public class WyszukiwaczPolaczenJednoliniowy(TransitDbContext db) : IWyszukiwaczPolaczen
{
    public async Task<IReadOnlyList<PolaczenieDto>> WyszukajAsync(
        int idPrzystankuZ,
        int idPrzystankuDo,
        DateOnly data,
        TimeOnly czas,
        int maxWynikow,
        CancellationToken ct = default)
    {
        // Wyznacz typ dnia (uproszczone: pn-pt = ROB, sb = SOB, nd = SWI)
        var typDniaKod = data.DayOfWeek switch
        {
            DayOfWeek.Saturday => "SOB",
            DayOfWeek.Sunday => "SWI",
            _ => "ROB"
        };

        // Znajdź pary (pwA, pwB) w tym samym wariancie, gdzie A jest przed B
        var pary = await (
            from pwA in db.PrzystankiWariantu
            join pwB in db.PrzystankiWariantu on pwA.IdWariantu equals pwB.IdWariantu
            join w in db.WariantyTras on pwA.IdWariantu equals w.IdWariantu
            join l in db.Linie on w.IdLinii equals l.IdLinii
            where pwA.IdPrzystanku == idPrzystankuZ
                  && pwB.IdPrzystanku == idPrzystankuDo
                  && pwA.Kolejnosc < pwB.Kolejnosc
                  && w.Aktywny && l.Aktywna
            select new
            {
                IdWariantu = w.IdWariantu,
                NazwaPrzystankuZ = pwA.Przystanek.Nazwa,
                NazwaPrzystankuDo = pwB.Przystanek.Nazwa,
                IdPwA = pwA.Id,
                IdPwB = pwB.Id,
                KolejnoscA = pwA.Kolejnosc,
                KolejnoscB = pwB.Kolejnosc,
                l.NumerLinii,
                l.TypLinii,
                w.Kierunek
            }
        ).ToListAsync(ct);

        if (pary.Count == 0) return [];

        var wyniki = new List<PolaczenieDto>();

        foreach (var para in pary)
        {
            // Znajdź odjazdy z przystanku A po podanym czasie
            var odjazdyA = await (
                from o in db.OdjazydPlanowe
                join r in db.RozkladyJazdy on o.IdRozkladu equals r.IdRozkladu
                join t in db.TypyDni on r.IdTypuDnia equals t.IdTypuDnia
                where o.IdPrzystankuWariantu == para.IdPwA
                      && t.Kod == typDniaKod
                      && r.Aktywny
                      && r.DataWaznosciOd <= data
                      && (r.DataWaznosciDo == null || r.DataWaznosciDo >= data)
                      && o.PlanowaGodzinaOdjazdu >= czas
                orderby o.PlanowaGodzinaOdjazdu
                select new { o.PlanowaGodzinaOdjazdu, o.NrKursu, o.IdRozkladu }
            ).Take(maxWynikow).ToListAsync(ct);

            foreach (var odjazdA in odjazdyA)
            {
                // Znajdź odjazd B z tego samego kursu i rozkładu
                var odjazdB = await db.OdjazydPlanowe
                    .Where(o => o.IdPrzystankuWariantu == para.IdPwB
                                && o.NrKursu == odjazdA.NrKursu
                                && o.IdRozkladu == odjazdA.IdRozkladu)
                    .Select(o => o.PlanowaGodzinaOdjazdu)
                    .FirstOrDefaultAsync(ct);

                if (odjazdB == default) continue;

                var czasTrwania = (int)(odjazdB - odjazdA.PlanowaGodzinaOdjazdu).TotalMinutes;
                if (czasTrwania < 0) continue;

                wyniki.Add(new PolaczenieDto(
                    para.NumerLinii,
                    para.TypLinii,
                    para.Kierunek,
                    para.NazwaPrzystankuZ,
                    para.NazwaPrzystankuDo,
                    odjazdA.PlanowaGodzinaOdjazdu,
                    odjazdB,
                    czasTrwania,
                    para.KolejnoscB - para.KolejnoscA - 1
                ));
            }
        }

        return wyniki.OrderBy(x => x.GodzinaOdjazdu).Take(maxWynikow).ToList();
    }
}
