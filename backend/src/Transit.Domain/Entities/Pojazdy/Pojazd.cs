using Transit.Domain.ValueObjects;

namespace Transit.Domain.Entities.Pojazdy;

public class Pojazd
{
    public int IdPojazdu { get; private set; }
    public int IdModelu { get; private set; }
    public string NumerBoczny { get; private set; } = default!;
    public string Vin { get; private set; } = default!;
    public int RokProdukcji { get; private set; }
    public DateOnly DataZakupu { get; private set; }
    public bool Aktywny { get; private set; } = true;

    public ModelPojazdu Model { get; private set; } = default!;
    public ICollection<Rozklady.RealizacjaKursu> Realizacje { get; private set; } = new List<Rozklady.RealizacjaKursu>();
    public ICollection<Rozklady.PrzegladUsterka> Przeglady { get; private set; } = new List<Rozklady.PrzegladUsterka>();
    public ICollection<Rozklady.GrafikPracy> Grafiki { get; private set; } = new List<Rozklady.GrafikPracy>();

    private Pojazd() { }

    public static Pojazd Utworz(int idModelu, string numerBoczny, string vin, int rokProdukcji, DateOnly dataZakupu)
    {
        var vinVo = new ValueObjects.Vin(vin);
        int biezacyRok = DateOnly.FromDateTime(DateTime.UtcNow).Year;
        if (rokProdukcji < 1950 || rokProdukcji > biezacyRok)
            throw new Exceptions.DomainException($"Rok produkcji {rokProdukcji} jest nieprawidłowy.");

        return new Pojazd
        {
            IdModelu = idModelu,
            NumerBoczny = numerBoczny,
            Vin = vinVo.Value,
            RokProdukcji = rokProdukcji,
            DataZakupu = dataZakupu
        };
    }

    public void Deaktywuj() => Aktywny = false;
    public void Aktywuj() => Aktywny = true;
}
