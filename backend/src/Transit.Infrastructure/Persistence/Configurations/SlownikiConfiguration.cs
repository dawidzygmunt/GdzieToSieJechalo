using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transit.Domain.Entities.Slowniki;

namespace Transit.Infrastructure.Persistence.Configurations;

public class TypDniaConfiguration : IEntityTypeConfiguration<TypDnia>
{
    public void Configure(EntityTypeBuilder<TypDnia> b)
    {
        b.ToTable("Typy_Dni");
        b.HasKey(x => x.IdTypuDnia);
        b.Property(x => x.IdTypuDnia).HasColumnName("id_typu_dnia");
        b.Property(x => x.Kod).HasColumnName("kod").HasMaxLength(3).IsRequired();
        b.Property(x => x.Nazwa).HasColumnName("nazwa").HasMaxLength(50).IsRequired();
        b.HasIndex(x => x.Kod).IsUnique();
    }
}

public class UprawnienieKategoriiConfiguration : IEntityTypeConfiguration<UprawnienieKategorii>
{
    public void Configure(EntityTypeBuilder<UprawnienieKategorii> b)
    {
        b.ToTable("Uprawnienia_Kategorii");
        b.HasKey(x => x.IdUprawnienia);
        b.Property(x => x.IdUprawnienia).HasColumnName("id_uprawnienia");
        b.Property(x => x.Kategoria).HasColumnName("kategoria").HasMaxLength(20).IsRequired();
        b.Property(x => x.Opis).HasColumnName("opis").HasMaxLength(200).IsRequired();
        b.HasIndex(x => x.Kategoria).IsUnique();
    }
}

public class KategoriaOplatyConfiguration : IEntityTypeConfiguration<KategoriaOplaty>
{
    public void Configure(EntityTypeBuilder<KategoriaOplaty> b)
    {
        b.ToTable("Kategorie_Biletow");
        b.HasKey(x => x.IdKategorii);
        b.Property(x => x.IdKategorii).HasColumnName("id_kategorii");
        b.Property(x => x.Nazwa).HasColumnName("nazwa").HasMaxLength(50).IsRequired();
        b.Property(x => x.ZnizkaPct).HasColumnName("znizka_pct").HasDefaultValue(0);
        b.HasIndex(x => x.Nazwa).IsUnique();
    }
}

public class TypPrzegladuConfiguration : IEntityTypeConfiguration<TypPrzegladu>
{
    public void Configure(EntityTypeBuilder<TypPrzegladu> b)
    {
        b.ToTable("Typy_Przegladu");
        b.HasKey(x => x.IdTypuPrzegladu);
        b.Property(x => x.IdTypuPrzegladu).HasColumnName("id_typu_przegladu");
        b.Property(x => x.Kod).HasColumnName("kod").HasMaxLength(10).IsRequired();
        b.Property(x => x.Nazwa).HasColumnName("nazwa").HasMaxLength(100).IsRequired();
        b.Property(x => x.InterwalDni).HasColumnName("interwal_dni");
        b.HasIndex(x => x.Kod).IsUnique();
    }
}

public class DzielnicaConfiguration : IEntityTypeConfiguration<Domain.Entities.Slowniki.Dzielnica>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Slowniki.Dzielnica> b)
    {
        b.ToTable("Dzielnice");
        b.HasKey(x => x.IdDzielnicy);
        b.Property(x => x.IdDzielnicy).HasColumnName("id_dzielnicy");
        b.Property(x => x.Nazwa).HasColumnName("nazwa").HasMaxLength(100).IsRequired();
        b.HasIndex(x => x.Nazwa).IsUnique();
    }
}
