namespace Transit.Domain.Entities.Slowniki;

public class KategoriaOplaty
{
    public int IdKategorii { get; private set; }
    public string Nazwa { get; private set; } = default!;
    public int ZnizkaPct { get; private set; }

    public ICollection<Pasazerowie.BiletyOkresowe> Bilety { get; private set; } = new List<Pasazerowie.BiletyOkresowe>();

    private KategoriaOplaty() { }

    public static KategoriaOplaty Utworz(string nazwa, int znizkaPct)
    {
        if (znizkaPct < 0 || znizkaPct > 100)
            throw new Exceptions.DomainException("Zniżka musi być w przedziale 0-100%.");
        return new KategoriaOplaty { Nazwa = nazwa, ZnizkaPct = znizkaPct };
    }
}
