using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Domain.Entities.Pasazerowie;
using Transit.Domain.Entities.Personel;
using Transit.Domain.Entities.Pojazdy;
using Transit.Domain.Entities.Rozklady;
using Transit.Domain.Entities.Siec;
using Transit.Domain.Entities.Slowniki;
using Transit.Infrastructure.Identity;

namespace Transit.Infrastructure.Persistence;

public class TransitDbContext(DbContextOptions<TransitDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, int>(options), IApplicationDbContext
{
    public DbSet<TypDnia> TypyDni => Set<TypDnia>();
    public DbSet<UprawnienieKategorii> UprawieniaKategorii => Set<UprawnienieKategorii>();
    public DbSet<KategoriaOplaty> KategorieOplat => Set<KategoriaOplaty>();
    public DbSet<TypPrzegladu> TypyPrzegladu => Set<TypPrzegladu>();
    public DbSet<Dzielnica> Dzielnice => Set<Dzielnica>();

    public DbSet<ProducentPojazdu> ProducenciPojazdow => Set<ProducentPojazdu>();
    public DbSet<ModelPojazdu> ModelePojazdow => Set<ModelPojazdu>();
    public DbSet<Pojazd> Pojazdy => Set<Pojazd>();

    public DbSet<Przystanek> Przystanki => Set<Przystanek>();
    public DbSet<Linia> Linie => Set<Linia>();
    public DbSet<WariantTrasy> WariantyTras => Set<WariantTrasy>();
    public DbSet<PrzystanekWariantu> PrzystankiWariantu => Set<PrzystanekWariantu>();

    public DbSet<Kierowca> Kierowcy => Set<Kierowca>();
    public DbSet<UprawnienieKierowcy> UprawieniaKierowcow => Set<UprawnienieKierowcy>();
    public DbSet<BadanieLekarskie> BadaniaLekarskie => Set<BadanieLekarskie>();
    public DbSet<Kontroler> Kontrolerzy => Set<Kontroler>();

    public DbSet<RozkladJazdy> RozkladyJazdy => Set<RozkladJazdy>();
    public DbSet<OdjazdPlanowy> OdjazydPlanowe => Set<OdjazdPlanowy>();
    public DbSet<RealizacjaKursu> RealizacjeKursow => Set<RealizacjaKursu>();
    public DbSet<DziennikPrzejazdu> DziennikPrzejazdow => Set<DziennikPrzejazdu>();
    public DbSet<GrafikPracy> GrafikiPracy => Set<GrafikPracy>();
    public DbSet<PrzegladUsterka> PrzegladyUsterki => Set<PrzegladUsterka>();

    public DbSet<Pasazer> Pasazerowie => Set<Pasazer>();
    public DbSet<BiletyOkresowe> BiletyOkresowe => Set<BiletyOkresowe>();
    public DbSet<KontrolaWPojedzie> KontrolWPojazdach => Set<KontrolaWPojedzie>();
    public DbSet<Mandat> Mandaty => Set<Mandat>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(TransitDbContext).Assembly);
    }
}
