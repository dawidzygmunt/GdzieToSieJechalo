namespace Transit.Domain.Entities.Pasazerowie;

public class KontrolaWPojedzie
{
    public int IdKontroli { get; private set; }
    public int IdKontrolera { get; private set; }
    public int IdRealizacji { get; private set; }
    public DateTime DataGodzina { get; private set; }
    public string Wynik { get; private set; } = default!;

    public Personel.Kontroler Kontroler { get; private set; } = default!;
    public Rozklady.RealizacjaKursu Realizacja { get; private set; } = default!;
    public ICollection<Mandat> Mandaty { get; private set; } = new List<Mandat>();

    private KontrolaWPojedzie() { }

    public static KontrolaWPojedzie Utworz(int idKontrolera, int idRealizacji, DateTime dataGodzina, string wynik) =>
        new()
        {
            IdKontrolera = idKontrolera,
            IdRealizacji = idRealizacji,
            DataGodzina = dataGodzina,
            Wynik = wynik
        };
}
