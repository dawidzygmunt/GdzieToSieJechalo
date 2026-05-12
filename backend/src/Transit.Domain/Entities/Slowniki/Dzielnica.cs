namespace Transit.Domain.Entities.Slowniki;

public class Dzielnica
{
    public int IdDzielnicy { get; private set; }
    public string Nazwa { get; private set; } = default!;

    public ICollection<Siec.Przystanek> Przystanki { get; private set; } = new List<Siec.Przystanek>();

    private Dzielnica() { }

    public static Dzielnica Utworz(string nazwa) => new() { Nazwa = nazwa };
}
