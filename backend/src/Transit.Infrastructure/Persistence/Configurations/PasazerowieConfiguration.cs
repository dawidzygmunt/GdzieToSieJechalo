using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transit.Domain.Entities.Pasazerowie;

namespace Transit.Infrastructure.Persistence.Configurations;

public class PasazerConfiguration : IEntityTypeConfiguration<Pasazer>
{
    public void Configure(EntityTypeBuilder<Pasazer> b)
    {
        b.ToTable("Pasazerowie");
        b.HasKey(x => x.IdPasazera);
        b.Property(x => x.IdPasazera).HasColumnName("id_pasazera");
        b.Property(x => x.Imie).HasColumnName("imie").HasMaxLength(50).IsRequired();
        b.Property(x => x.Nazwisko).HasColumnName("nazwisko").HasMaxLength(50).IsRequired();
        b.Property(x => x.Pesel).HasColumnName("pesel").HasMaxLength(11);
        b.Property(x => x.Email).HasColumnName("email").HasMaxLength(100);
        b.HasIndex(x => x.Pesel).IsUnique().HasFilter("pesel IS NOT NULL");
        b.HasIndex(x => x.Email).IsUnique().HasFilter("email IS NOT NULL");
        b.Ignore(x => x.PelneNazwisko);
    }
}

public class BiletyOkresoweCfg : IEntityTypeConfiguration<BiletyOkresowe>
{
    public void Configure(EntityTypeBuilder<BiletyOkresowe> b)
    {
        b.ToTable("Bilety_Okresowe");
        b.HasKey(x => x.IdBiletu);
        b.Property(x => x.IdBiletu).HasColumnName("id_biletu");
        b.Property(x => x.IdPasazera).HasColumnName("id_pasazera");
        b.Property(x => x.IdKategorii).HasColumnName("id_kategorii");
        b.Property(x => x.DataOd).HasColumnName("data_od");
        b.Property(x => x.DataDo).HasColumnName("data_do");
        b.Property(x => x.Cena).HasColumnName("cena").HasColumnType("decimal(8,2)");
        b.Property(x => x.Aktywny).HasColumnName("aktywny").HasDefaultValue(true);
        b.HasOne(x => x.Pasazer).WithMany(x => x.Bilety).HasForeignKey(x => x.IdPasazera);
        b.HasOne(x => x.Kategoria).WithMany(x => x.Bilety).HasForeignKey(x => x.IdKategorii);
    }
}

public class KontrolaWPojedzeConfiguration : IEntityTypeConfiguration<KontrolaWPojedzie>
{
    public void Configure(EntityTypeBuilder<KontrolaWPojedzie> b)
    {
        b.ToTable("Kontrole_W_Pojazdach");
        b.HasKey(x => x.IdKontroli);
        b.Property(x => x.IdKontroli).HasColumnName("id_kontroli");
        b.Property(x => x.IdKontrolera).HasColumnName("id_kontrolera");
        b.Property(x => x.IdRealizacji).HasColumnName("id_realizacji");
        b.Property(x => x.DataGodzina).HasColumnName("data_godzina");
        b.Property(x => x.Wynik).HasColumnName("wynik").HasMaxLength(30).IsRequired();
        b.HasOne(x => x.Kontroler).WithMany(x => x.Kontrole).HasForeignKey(x => x.IdKontrolera);
        b.HasOne(x => x.Realizacja).WithMany(x => x.Kontrole).HasForeignKey(x => x.IdRealizacji);
    }
}

public class MandatConfiguration : IEntityTypeConfiguration<Mandat>
{
    public void Configure(EntityTypeBuilder<Mandat> b)
    {
        b.ToTable("Mandaty");
        b.HasKey(x => x.IdMandatu);
        b.Property(x => x.IdMandatu).HasColumnName("id_mandatu");
        b.Property(x => x.IdKontroli).HasColumnName("id_kontroli");
        b.Property(x => x.IdPasazera).HasColumnName("id_pasazera");
        b.Property(x => x.NrDokumentuPasazera).HasColumnName("nr_dokumentu_pasazera").HasMaxLength(20);
        b.Property(x => x.Kwota).HasColumnName("kwota").HasColumnType("decimal(8,2)");
        b.Property(x => x.Powod).HasColumnName("powod").HasMaxLength(200).IsRequired();
        b.Property(x => x.DataPlatnosci).HasColumnName("data_platnosci");
        b.Property(x => x.Oplacony).HasColumnName("oplacony").HasDefaultValue(false);
        b.HasOne(x => x.Kontrola).WithMany(x => x.Mandaty).HasForeignKey(x => x.IdKontroli);
        b.HasOne(x => x.Pasazer).WithMany(x => x.Mandaty).HasForeignKey(x => x.IdPasazera).IsRequired(false);
    }
}
