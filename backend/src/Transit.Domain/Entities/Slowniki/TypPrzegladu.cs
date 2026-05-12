namespace Transit.Domain.Entities.Slowniki;

public class TypPrzegladu
{
    public int IdTypuPrzegladu { get; private set; }
    public string Kod { get; private set; } = default!;
    public string Nazwa { get; private set; } = default!;
    public int InterwalDni { get; private set; }

    public ICollection<Rozklady.PrzegladUsterka> Przeglady { get; private set; } = new List<Rozklady.PrzegladUsterka>();

    private TypPrzegladu() { }

    public static TypPrzegladu Utworz(string kod, string nazwa, int interwalDni)
    {
        if (interwalDni <= 0)
            throw new Exceptions.DomainException("Interwał przeglądu musi być dodatni.");
        return new TypPrzegladu { Kod = kod, Nazwa = nazwa, InterwalDni = interwalDni };
    }

    public DateOnly ObliczNastepnyTermin(DateOnly dataPrzegladu) =>
        dataPrzegladu.AddDays(InterwalDni);
}
