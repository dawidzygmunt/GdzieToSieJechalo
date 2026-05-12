using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transit.Domain.Entities.Pojazdy;

namespace Transit.Infrastructure.Persistence.Configurations;

public class ProducentPojazduConfiguration : IEntityTypeConfiguration<ProducentPojazdu>
{
    public void Configure(EntityTypeBuilder<ProducentPojazdu> b)
    {
        b.ToTable("Producenci_Pojazdow");
        b.HasKey(x => x.IdProducenta);
        b.Property(x => x.IdProducenta).HasColumnName("id_producenta");
        b.Property(x => x.Nazwa).HasColumnName("nazwa").HasMaxLength(100).IsRequired();
        b.HasIndex(x => x.Nazwa).IsUnique();
    }
}

public class ModelPojazduConfiguration : IEntityTypeConfiguration<ModelPojazdu>
{
    public void Configure(EntityTypeBuilder<ModelPojazdu> b)
    {
        b.ToTable("Modele_Pojazdow");
        b.HasKey(x => x.IdModelu);
        b.Property(x => x.IdModelu).HasColumnName("id_modelu");
        b.Property(x => x.IdProducenta).HasColumnName("id_producenta");
        b.Property(x => x.NazwaModelu).HasColumnName("nazwa_modelu").HasMaxLength(100).IsRequired();
        b.Property(x => x.TypPojazdu).HasColumnName("typ_pojazdu").HasMaxLength(20).IsRequired();
        b.Property(x => x.LiczbaMiejsc).HasColumnName("liczba_miejsc");
        b.HasOne(x => x.Producent).WithMany(x => x.Modele).HasForeignKey(x => x.IdProducenta);
    }
}

public class PojazdConfiguration : IEntityTypeConfiguration<Pojazd>
{
    public void Configure(EntityTypeBuilder<Pojazd> b)
    {
        b.ToTable("Pojazdy");
        b.HasKey(x => x.IdPojazdu);
        b.Property(x => x.IdPojazdu).HasColumnName("id_pojazdu");
        b.Property(x => x.IdModelu).HasColumnName("id_modelu");
        b.Property(x => x.NumerBoczny).HasColumnName("numer_boczny").HasMaxLength(10).IsRequired();
        b.Property(x => x.Vin).HasColumnName("vin").HasMaxLength(17).IsRequired();
        b.Property(x => x.RokProdukcji).HasColumnName("rok_produkcji");
        b.Property(x => x.DataZakupu).HasColumnName("data_zakupu");
        b.Property(x => x.Aktywny).HasColumnName("aktywny").HasDefaultValue(true);
        b.HasIndex(x => x.NumerBoczny).IsUnique();
        b.HasIndex(x => x.Vin).IsUnique();
        b.HasOne(x => x.Model).WithMany(x => x.Pojazdy).HasForeignKey(x => x.IdModelu);
    }
}
