namespace Transit.Domain.Entities.Slowniki;

public class TypDnia
{
    public int IdTypuDnia { get; private set; }
    public string Kod { get; private set; } = default!;
    public string Nazwa { get; private set; } = default!;

    public ICollection<Rozklady.RozkladJazdy> Rozklady { get; private set; } = new List<Rozklady.RozkladJazdy>();

    private TypDnia() { }

    public static TypDnia Utworz(string kod, string nazwa)
    {
        if (string.IsNullOrWhiteSpace(kod) || kod.Length > 3)
            throw new Exceptions.DomainException("Kod typu dnia musi mieć 1-3 znaki.");
        return new TypDnia { Kod = kod.ToUpperInvariant(), Nazwa = nazwa };
    }
}
