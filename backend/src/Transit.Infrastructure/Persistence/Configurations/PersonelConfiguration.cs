using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transit.Domain.Entities.Personel;

namespace Transit.Infrastructure.Persistence.Configurations;

public class KierowcaConfiguration : IEntityTypeConfiguration<Kierowca>
{
    public void Configure(EntityTypeBuilder<Kierowca> b)
    {
        b.ToTable("Kierowcy");
        b.HasKey(x => x.IdKierowcy);
        b.Property(x => x.IdKierowcy).HasColumnName("id_kierowcy");
        b.Property(x => x.Imie).HasColumnName("imie").HasMaxLength(50).IsRequired();
        b.Property(x => x.Nazwisko).HasColumnName("nazwisko").HasMaxLength(50).IsRequired();
        b.Property(x => x.NrPracownika).HasColumnName("nr_pracownika").HasMaxLength(20).IsRequired();
        b.Property(x => x.DataZatrudnienia).HasColumnName("data_zatrudnienia");
        b.Property(x => x.Aktywny).HasColumnName("aktywny").HasDefaultValue(true);
        b.HasIndex(x => x.NrPracownika).IsUnique();
        b.Ignore(x => x.PelneNazwisko);
    }
}

public class UprawnienieKierowcyConfiguration : IEntityTypeConfiguration<UprawnienieKierowcy>
{
    public void Configure(EntityTypeBuilder<UprawnienieKierowcy> b)
    {
        b.ToTable("Uprawnienia_Kierowcow");
        b.HasKey(x => x.Id);
        b.Property(x => x.IdKierowcy).HasColumnName("id_kierowcy");
        b.Property(x => x.IdUprawnienia).HasColumnName("id_uprawnienia");
        b.Property(x => x.DataUzyskania).HasColumnName("data_uzyskania");
        b.Property(x => x.DataWaznosci).HasColumnName("data_waznosci");
        b.HasOne(x => x.Kierowca).WithMany(x => x.Uprawnienia).HasForeignKey(x => x.IdKierowcy);
        b.HasOne(x => x.UprawnienieKategorii).WithMany(x => x.UprawnieniKierowcow).HasForeignKey(x => x.IdUprawnienia);
    }
}

public class BadanieLekarskieConfiguration : IEntityTypeConfiguration<BadanieLekarskie>
{
    public void Configure(EntityTypeBuilder<BadanieLekarskie> b)
    {
        b.ToTable("Badania_Lekarskie");
        b.HasKey(x => x.IdBadania);
        b.Property(x => x.IdBadania).HasColumnName("id_badania");
        b.Property(x => x.IdKierowcy).HasColumnName("id_kierowcy");
        b.Property(x => x.DataBadania).HasColumnName("data_badania");
        b.Property(x => x.DataWaznosci).HasColumnName("data_waznosci");
        b.Property(x => x.Wynik).HasColumnName("wynik").HasMaxLength(10).IsRequired();
        b.Property(x => x.Lekarz).HasColumnName("lekarz").HasMaxLength(100).IsRequired();
        b.HasOne(x => x.Kierowca).WithMany(x => x.Badania).HasForeignKey(x => x.IdKierowcy);
    }
}

public class KontrolerConfiguration : IEntityTypeConfiguration<Kontroler>
{
    public void Configure(EntityTypeBuilder<Kontroler> b)
    {
        b.ToTable("Kontrolerzy");
        b.HasKey(x => x.IdKontrolera);
        b.Property(x => x.IdKontrolera).HasColumnName("id_kontrolera");
        b.Property(x => x.Imie).HasColumnName("imie").HasMaxLength(50).IsRequired();
        b.Property(x => x.Nazwisko).HasColumnName("nazwisko").HasMaxLength(50).IsRequired();
        b.Property(x => x.NrSluzbowy).HasColumnName("nr_sluzbowy").HasMaxLength(20).IsRequired();
        b.Property(x => x.Aktywny).HasColumnName("aktywny").HasDefaultValue(true);
        b.HasIndex(x => x.NrSluzbowy).IsUnique();
        b.Ignore(x => x.PelneNazwisko);
    }
}
