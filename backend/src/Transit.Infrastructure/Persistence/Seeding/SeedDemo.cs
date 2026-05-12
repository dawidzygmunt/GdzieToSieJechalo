using Microsoft.EntityFrameworkCore;
using Transit.Domain.Entities.Personel;
using Transit.Domain.Entities.Pojazdy;
using Transit.Domain.Entities.Siec;

namespace Transit.Infrastructure.Persistence.Seeding;

public static class SeedDemo
{
    public static async Task SeedAsync(TransitDbContext db)
    {
        if (await db.Dzielnice.AnyAsync()) return;

        // Dzielnice
        var srodmiescie = Domain.Entities.Slowniki.Dzielnica.Utworz("Śródmieście");
        var mokotow = Domain.Entities.Slowniki.Dzielnica.Utworz("Mokotów");
        db.Dzielnice.AddRange(srodmiescie, mokotow);
        await db.SaveChangesAsync();

        // Przystanki
        var p1 = Przystanek.Utworz(srodmiescie.IdDzielnicy, "Centrum", "ul. Marszałkowska 1", "naziemny", true);
        var p2 = Przystanek.Utworz(srodmiescie.IdDzielnicy, "Teatr Wielki", "ul. Senatorska 35", "naziemny", true);
        var p3 = Przystanek.Utworz(mokotow.IdDzielnicy, "Plac Unii", "ul. Puławska 2", "naziemny", false);
        var p4 = Przystanek.Utworz(mokotow.IdDzielnicy, "Wiśniowa", "ul. Wiśniowa 10", "naziemny", false);
        var p5 = Przystanek.Utworz(srodmiescie.IdDzielnicy, "Politechnika", "ul. Noakowskiego 4", "naziemny", true);
        db.Przystanki.AddRange(p1, p2, p3, p4, p5);
        await db.SaveChangesAsync();

        // Producent + model + pojazdy
        var producent = ProducentPojazdu.Utworz("Solaris Bus & Coach");
        db.ProducenciPojazdow.Add(producent);
        await db.SaveChangesAsync();

        var model = ModelPojazdu.Utworz(producent.IdProducenta, "Urbino 12", "autobus", 75);
        db.ModelePojazdow.Add(model);
        await db.SaveChangesAsync();

        var poj1 = Pojazd.Utworz(model.IdModelu, "1001", "WH1234567890ABCDE", 2020, new DateOnly(2020, 3, 15));
        var poj2 = Pojazd.Utworz(model.IdModelu, "1002", "WH1234567890ABCDF", 2021, new DateOnly(2021, 6, 10));
        db.Pojazdy.AddRange(poj1, poj2);
        await db.SaveChangesAsync();

        // Kierowca
        var k1 = Kierowca.Utworz("Jan", "Kowalski", "KIE001", new DateOnly(2015, 1, 10));
        var k2 = Kierowca.Utworz("Anna", "Nowak", "KIE002", new DateOnly(2018, 5, 20));
        db.Kierowcy.AddRange(k1, k2);
        await db.SaveChangesAsync();

        // Linia 180: Centrum → Mokotów (5 przystanków)
        var linia = Linia.Utworz("180", "autobus", "Centrum - Mokotów");
        db.Linie.Add(linia);
        await db.SaveChangesAsync();

        var wariant = WariantTrasy.Utworz(linia.IdLinii, "A", "Centrum → Plac Unii");
        db.WariantyTras.Add(wariant);
        await db.SaveChangesAsync();

        var pw1 = PrzystanekWariantu.Utworz(wariant.IdWariantu, p1.IdPrzystanku, 1, 0);
        var pw2 = PrzystanekWariantu.Utworz(wariant.IdWariantu, p2.IdPrzystanku, 2, 800);
        var pw3 = PrzystanekWariantu.Utworz(wariant.IdWariantu, p5.IdPrzystanku, 3, 1600);
        var pw4 = PrzystanekWariantu.Utworz(wariant.IdWariantu, p4.IdPrzystanku, 4, 3000);
        var pw5 = PrzystanekWariantu.Utworz(wariant.IdWariantu, p3.IdPrzystanku, 5, 4200);
        db.PrzystankiWariantu.AddRange(pw1, pw2, pw3, pw4, pw5);
        await db.SaveChangesAsync();

        // Rozkład roboczy
        var typRob = await db.TypyDni.FirstAsync(t => t.Kod == "ROB");
        var rozklad = Domain.Entities.Rozklady.RozkladJazdy.Utworz(
            wariant.IdWariantu, typRob.IdTypuDnia, new DateOnly(2026, 1, 1));
        db.RozkladyJazdy.Add(rozklad);
        await db.SaveChangesAsync();

        // 3 kursy po 5 przystanków
        var kursy = new[]
        {
            (nr: 1, godz: new TimeOnly(6, 0)),
            (nr: 2, godz: new TimeOnly(7, 0)),
            (nr: 3, godz: new TimeOnly(8, 0))
        };
        var przystankiWariantu = new[] { pw1, pw2, pw3, pw4, pw5 };
        var offsetyMin = new[] { 0, 3, 6, 11, 15 };

        foreach (var (nr, godz) in kursy)
        {
            for (int i = 0; i < przystankiWariantu.Length; i++)
            {
                var odjazd = Domain.Entities.Rozklady.OdjazdPlanowy.Utworz(
                    rozklad.IdRozkladu,
                    przystankiWariantu[i].Id,
                    godz.AddMinutes(offsetyMin[i]),
                    nr);
                db.OdjazydPlanowe.Add(odjazd);
            }
        }

        await db.SaveChangesAsync();
    }
}
