namespace Transit.Domain.Entities.Siec;

public class WariantTrasy
{
    public int IdWariantu { get; private set; }
    public int IdLinii { get; private set; }
    public string NazwaWariantu { get; private set; } = default!;
    public string Kierunek { get; private set; } = default!;
    public bool Aktywny { get; private set; } = true;

    public Linia Linia { get; private set; } = default!;
    public ICollection<PrzystanekWariantu> PrzystankiWariantu { get; private set; } = new List<PrzystanekWariantu>();
    public ICollection<Rozklady.RozkladJazdy> Rozklady { get; private set; } = new List<Rozklady.RozkladJazdy>();
    public ICollection<Rozklady.RealizacjaKursu> Realizacje { get; private set; } = new List<Rozklady.RealizacjaKursu>();

    private WariantTrasy() { }

    public static WariantTrasy Utworz(int idLinii, string nazwaWariantu, string kierunek) =>
        new() { IdLinii = idLinii, NazwaWariantu = nazwaWariantu, Kierunek = kierunek };

    public void Deaktywuj() => Aktywny = false;
}
