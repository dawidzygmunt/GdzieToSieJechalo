using Microsoft.EntityFrameworkCore;
using Transit.Domain.Entities.Slowniki;

namespace Transit.Infrastructure.Persistence.Seeding;

public static class SeedSlownikow
{
    public static async Task SeedAsync(TransitDbContext db)
    {
        if (!await db.TypyDni.AnyAsync())
        {
            db.TypyDni.AddRange(
                TypDnia.Utworz("ROB", "Roboczy (Pn-Pt)"),
                TypDnia.Utworz("SOB", "Sobota"),
                TypDnia.Utworz("SWI", "Niedziela/Święto")
            );
        }

        if (!await db.UprawieniaKategorii.AnyAsync())
        {
            db.UprawieniaKategorii.AddRange(
                UprawnienieKategorii.Utworz("D", "Prawo jazdy kat. D - autobus"),
                UprawnienieKategorii.Utworz("D+E", "Prawo jazdy kat. D+E - autobus z przyczepą"),
                UprawnienieKategorii.Utworz("T", "Uprawnienia do prowadzenia tramwaju")
            );
        }

        if (!await db.KategorieOplat.AnyAsync())
        {
            db.KategorieOplat.AddRange(
                KategoriaOplaty.Utworz("Normalny", 0),
                KategoriaOplaty.Utworz("Ulgowy 50%", 50),
                KategoriaOplaty.Utworz("Bezpłatny", 100)
            );
        }

        if (!await db.TypyPrzegladu.AnyAsync())
        {
            db.TypyPrzegladu.AddRange(
                TypPrzegladu.Utworz("OKR", "Przegląd okresowy", 365),
                TypPrzegladu.Utworz("TECH", "Przegląd techniczny", 180),
                TypPrzegladu.Utworz("AWA", "Przegląd awaryjny", 30)
            );
        }

        await db.SaveChangesAsync();
    }
}
