using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transit.Domain.Entities.Rozklady;

namespace Transit.Infrastructure.Persistence.Configurations;

public class RozkladJazdyConfiguration : IEntityTypeConfiguration<RozkladJazdy>
{
    public void Configure(EntityTypeBuilder<RozkladJazdy> b)
    {
        b.ToTable("Rozklady_Jazdy");
        b.HasKey(x => x.IdRozkladu);
        b.Property(x => x.IdRozkladu).HasColumnName("id_rozkladu");
        b.Property(x => x.IdWariantu).HasColumnName("id_wariantu");
        b.Property(x => x.IdTypuDnia).HasColumnName("id_typu_dnia");
        b.Property(x => x.DataWaznosciOd).HasColumnName("data_waznosci_od");
        b.Property(x => x.DataWaznosciDo).HasColumnName("data_waznosci_do");
        b.Property(x => x.Aktywny).HasColumnName("aktywny").HasDefaultValue(true);
        b.HasOne(x => x.Wariant).WithMany(x => x.Rozklady).HasForeignKey(x => x.IdWariantu);
        b.HasOne(x => x.TypDnia).WithMany(x => x.Rozklady).HasForeignKey(x => x.IdTypuDnia);
    }
}

public class OdjazdPlanowyCfg : IEntityTypeConfiguration<OdjazdPlanowy>
{
    public void Configure(EntityTypeBuilder<OdjazdPlanowy> b)
    {
        b.ToTable("Odjazdy_Planowe");
        b.HasKey(x => x.IdOdjazdu);
        b.Property(x => x.IdOdjazdu).HasColumnName("id_odjazdu");
        b.Property(x => x.IdRozkladu).HasColumnName("id_rozkladu");
        b.Property(x => x.IdPrzystankuWariantu).HasColumnName("id_przystanku_wariantu");
        b.Property(x => x.PlanowaGodzinaOdjazdu).HasColumnName("planowa_godzina_odjazdu").HasColumnType("time");
        b.Property(x => x.OffsetDni).HasColumnName("offset_dni").HasDefaultValue((short)0);
        b.Property(x => x.NrKursu).HasColumnName("nr_kursu");
        b.HasIndex(x => new { x.IdRozkladu, x.IdPrzystankuWariantu });
        b.HasOne(x => x.Rozklad).WithMany(x => x.Odjazdy).HasForeignKey(x => x.IdRozkladu);
        b.HasOne(x => x.PrzystanekWariantu).WithMany(x => x.Odjazdy).HasForeignKey(x => x.IdPrzystankuWariantu);
    }
}

public class RealizacjaKursuConfiguration : IEntityTypeConfiguration<RealizacjaKursu>
{
    public void Configure(EntityTypeBuilder<RealizacjaKursu> b)
    {
        b.ToTable("Realizacja_Kursu");
        b.HasKey(x => x.IdRealizacji);
        b.Property(x => x.IdRealizacji).HasColumnName("id_realizacji");
        b.Property(x => x.IdWariantu).HasColumnName("id_wariantu");
        b.Property(x => x.IdKierowcy).HasColumnName("id_kierowcy");
        b.Property(x => x.IdPojazdu).HasColumnName("id_pojazdu");
        b.Property(x => x.DataKursu).HasColumnName("data_kursu");
        b.Property(x => x.NrKursu).HasColumnName("nr_kursu");
        b.Property(x => x.StatusKursu).HasColumnName("status_kursu").HasMaxLength(20);
        b.HasIndex(x => new { x.IdWariantu, x.DataKursu, x.NrKursu }).IsUnique();
        b.HasOne(x => x.Wariant).WithMany(x => x.Realizacje).HasForeignKey(x => x.IdWariantu);
        b.HasOne(x => x.Kierowca).WithMany(x => x.Realizacje).HasForeignKey(x => x.IdKierowcy).IsRequired(false);
        b.HasOne(x => x.Pojazd).WithMany(x => x.Realizacje).HasForeignKey(x => x.IdPojazdu).IsRequired(false);
    }
}

public class DziennikPrzejazduConfiguration : IEntityTypeConfiguration<DziennikPrzejazdu>
{
    public void Configure(EntityTypeBuilder<DziennikPrzejazdu> b)
    {
        b.ToTable("Dziennik_Przejazdu");
        b.HasKey(x => x.IdWpisu);
        b.Property(x => x.IdWpisu).HasColumnName("id_wpisu");
        b.Property(x => x.IdRealizacji).HasColumnName("id_realizacji");
        b.Property(x => x.IdOdjazdu).HasColumnName("id_odjazdu");
        b.Property(x => x.RzeczywiistyOdjazd).HasColumnName("rzeczywisty_odjazd");
        b.Property(x => x.RzeczywiistyPrzyjazd).HasColumnName("rzeczywisty_przyjazd");
        b.Property(x => x.OpoznienieMin).HasColumnName("opoznienie_min");
        b.Property(x => x.OffsetDni).HasColumnName("offset_dni").HasDefaultValue((short)0);
        b.HasOne(x => x.Realizacja).WithMany(x => x.Dzienniki).HasForeignKey(x => x.IdRealizacji);
        b.HasOne(x => x.Odjazd).WithMany(x => x.Dzienniki).HasForeignKey(x => x.IdOdjazdu);
    }
}

public class GrafikPracyConfiguration : IEntityTypeConfiguration<GrafikPracy>
{
    public void Configure(EntityTypeBuilder<GrafikPracy> b)
    {
        b.ToTable("Grafiki_Pracy");
        b.HasKey(x => x.IdGrafiku);
        b.Property(x => x.IdGrafiku).HasColumnName("id_grafiku");
        b.Property(x => x.IdKierowcy).HasColumnName("id_kierowcy");
        b.Property(x => x.IdPojazdu).HasColumnName("id_pojazdu");
        b.Property(x => x.Data).HasColumnName("data");
        b.Property(x => x.GodzinaOd).HasColumnName("godzina_od").HasColumnType("time");
        b.Property(x => x.GodzinaDo).HasColumnName("godzina_do").HasColumnType("time");
        b.Property(x => x.OffsetDni).HasColumnName("offset_dni").HasDefaultValue((short)0);
        b.HasOne(x => x.Kierowca).WithMany(x => x.Grafiki).HasForeignKey(x => x.IdKierowcy);
        b.HasOne(x => x.Pojazd).WithMany(x => x.Grafiki).HasForeignKey(x => x.IdPojazdu);
    }
}

public class PrzegladUsterkaConfiguration : IEntityTypeConfiguration<PrzegladUsterka>
{
    public void Configure(EntityTypeBuilder<PrzegladUsterka> b)
    {
        b.ToTable("Przeglady_Usterki");
        b.HasKey(x => x.IdPrzegladu);
        b.Property(x => x.IdPrzegladu).HasColumnName("id_przegladu");
        b.Property(x => x.IdPojazdu).HasColumnName("id_pojazdu");
        b.Property(x => x.IdTypuPrzegladu).HasColumnName("id_typu_przegladu");
        b.Property(x => x.DataPrzegladu).HasColumnName("data_przegladu");
        b.Property(x => x.Wynik).HasColumnName("wynik").HasMaxLength(20).IsRequired();
        b.Property(x => x.Uwagi).HasColumnName("uwagi").HasMaxLength(500);
        b.Property(x => x.Warsztat).HasColumnName("warsztat").HasMaxLength(100).IsRequired();
        b.HasOne(x => x.Pojazd).WithMany(x => x.Przeglady).HasForeignKey(x => x.IdPojazdu);
        b.HasOne(x => x.TypPrzegladu).WithMany(x => x.Przeglady).HasForeignKey(x => x.IdTypuPrzegladu);
    }
}
