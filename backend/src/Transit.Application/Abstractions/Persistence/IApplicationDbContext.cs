using Microsoft.EntityFrameworkCore;
using Transit.Domain.Entities.Pasazerowie;
using Transit.Domain.Entities.Personel;
using Transit.Domain.Entities.Pojazdy;
using Transit.Domain.Entities.Rozklady;
using Transit.Domain.Entities.Siec;
using Transit.Domain.Entities.Slowniki;

namespace Transit.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    DbSet<TypDnia> TypyDni { get; }
    DbSet<UprawnienieKategorii> UprawieniaKategorii { get; }
    DbSet<KategoriaOplaty> KategorieOplat { get; }
    DbSet<TypPrzegladu> TypyPrzegladu { get; }
    DbSet<Dzielnica> Dzielnice { get; }

    DbSet<ProducentPojazdu> ProducenciPojazdow { get; }
    DbSet<ModelPojazdu> ModelePojazdow { get; }
    DbSet<Pojazd> Pojazdy { get; }

    DbSet<Przystanek> Przystanki { get; }
    DbSet<Linia> Linie { get; }
    DbSet<WariantTrasy> WariantyTras { get; }
    DbSet<PrzystanekWariantu> PrzystankiWariantu { get; }

    DbSet<Kierowca> Kierowcy { get; }
    DbSet<UprawnienieKierowcy> UprawieniaKierowcow { get; }
    DbSet<BadanieLekarskie> BadaniaLekarskie { get; }
    DbSet<Kontroler> Kontrolerzy { get; }

    DbSet<RozkladJazdy> RozkladyJazdy { get; }
    DbSet<OdjazdPlanowy> OdjazydPlanowe { get; }
    DbSet<RealizacjaKursu> RealizacjeKursow { get; }
    DbSet<DziennikPrzejazdu> DziennikPrzejazdow { get; }
    DbSet<GrafikPracy> GrafikiPracy { get; }
    DbSet<PrzegladUsterka> PrzegladyUsterki { get; }

    DbSet<Pasazer> Pasazerowie { get; }
    DbSet<BiletyOkresowe> BiletyOkresowe { get; }
    DbSet<KontrolaWPojedzie> KontrolWPojazdach { get; }
    DbSet<Mandat> Mandaty { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
