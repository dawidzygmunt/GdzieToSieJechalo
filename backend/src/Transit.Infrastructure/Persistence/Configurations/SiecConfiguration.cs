using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transit.Domain.Entities.Siec;

namespace Transit.Infrastructure.Persistence.Configurations;

public class PrzystanekConfiguration : IEntityTypeConfiguration<Przystanek>
{
    public void Configure(EntityTypeBuilder<Przystanek> b)
    {
        b.ToTable("Przystanki");
        b.HasKey(x => x.IdPrzystanku);
        b.Property(x => x.IdPrzystanku).HasColumnName("id_przystanku");
        b.Property(x => x.IdDzielnicy).HasColumnName("id_dzielnicy");
        b.Property(x => x.Nazwa).HasColumnName("nazwa").HasMaxLength(100).IsRequired();
        b.Property(x => x.Ulica).HasColumnName("ulica").HasMaxLength(100).IsRequired();
        b.Property(x => x.Typ).HasColumnName("typ").HasMaxLength(20).HasDefaultValue("naziemny");
        b.Property(x => x.Wiata).HasColumnName("wiata").HasDefaultValue(false);
        b.Property(x => x.Aktywny).HasColumnName("aktywny").HasDefaultValue(true);
        b.HasOne(x => x.Dzielnica).WithMany(x => x.Przystanki).HasForeignKey(x => x.IdDzielnicy);
    }
}

public class LiniaConfiguration : IEntityTypeConfiguration<Linia>
{
    public void Configure(EntityTypeBuilder<Linia> b)
    {
        b.ToTable("Linie");
        b.HasKey(x => x.IdLinii);
        b.Property(x => x.IdLinii).HasColumnName("id_linii");
        b.Property(x => x.NumerLinii).HasColumnName("numer_linii").HasMaxLength(10).IsRequired();
        b.Property(x => x.TypLinii).HasColumnName("typ_linii").HasMaxLength(20).IsRequired();
        b.Property(x => x.Opis).HasColumnName("opis").HasMaxLength(200);
        b.Property(x => x.Aktywna).HasColumnName("aktywna").HasDefaultValue(true);
        b.HasIndex(x => x.NumerLinii).IsUnique();
    }
}

public class WariantTrasyConfiguration : IEntityTypeConfiguration<WariantTrasy>
{
    public void Configure(EntityTypeBuilder<WariantTrasy> b)
    {
        b.ToTable("Warianty_Trasy");
        b.HasKey(x => x.IdWariantu);
        b.Property(x => x.IdWariantu).HasColumnName("id_wariantu");
        b.Property(x => x.IdLinii).HasColumnName("id_linii");
        b.Property(x => x.NazwaWariantu).HasColumnName("nazwa_wariantu").HasMaxLength(50).IsRequired();
        b.Property(x => x.Kierunek).HasColumnName("kierunek").HasMaxLength(100).IsRequired();
        b.Property(x => x.Aktywny).HasColumnName("aktywny").HasDefaultValue(true);
        b.HasIndex(x => new { x.IdLinii, x.NazwaWariantu }).IsUnique();
        b.HasOne(x => x.Linia).WithMany(x => x.Warianty).HasForeignKey(x => x.IdLinii);
    }
}

public class PrzystanekWariantuConfiguration : IEntityTypeConfiguration<PrzystanekWariantu>
{
    public void Configure(EntityTypeBuilder<PrzystanekWariantu> b)
    {
        b.ToTable("Przystanki_Wariantu");
        b.HasKey(x => x.Id);
        b.Property(x => x.IdWariantu).HasColumnName("id_wariantu");
        b.Property(x => x.IdPrzystanku).HasColumnName("id_przystanku");
        b.Property(x => x.Kolejnosc).HasColumnName("kolejnosc");
        b.Property(x => x.OdlegloscOdPoczatkuM).HasColumnName("odleglosc_od_poczatku_m").HasDefaultValue(0);
        b.HasIndex(x => new { x.IdWariantu, x.Kolejnosc }).IsUnique();
        b.HasIndex(x => new { x.IdWariantu, x.IdPrzystanku }).IsUnique();
        b.HasOne(x => x.Wariant).WithMany(x => x.PrzystankiWariantu).HasForeignKey(x => x.IdWariantu);
        b.HasOne(x => x.Przystanek).WithMany(x => x.PrzystankiWariantu).HasForeignKey(x => x.IdPrzystanku);
    }
}
