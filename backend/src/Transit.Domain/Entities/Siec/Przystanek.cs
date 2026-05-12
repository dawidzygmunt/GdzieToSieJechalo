namespace Transit.Domain.Entities.Siec;

public class Przystanek
{
    public int IdPrzystanku { get; private set; }
    public int IdDzielnicy { get; private set; }
    public string Nazwa { get; private set; } = default!;
    public string Ulica { get; private set; } = default!;
    public string Typ { get; private set; } = "naziemny";
    public bool Wiata { get; private set; }
    public bool Aktywny { get; private set; } = true;

    public Slowniki.Dzielnica Dzielnica { get; private set; } = default!;
    public ICollection<PrzystanekWariantu> PrzystankiWariantu { get; private set; } = new List<PrzystanekWariantu>();

    private Przystanek() { }

    public static Przystanek Utworz(int idDzielnicy, string nazwa, string ulica, string typ = "naziemny", bool wiata = false) =>
        new()
        {
            IdDzielnicy = idDzielnicy,
            Nazwa = nazwa,
            Ulica = ulica,
            Typ = typ,
            Wiata = wiata
        };

    public void Deaktywuj() => Aktywny = false;
}
