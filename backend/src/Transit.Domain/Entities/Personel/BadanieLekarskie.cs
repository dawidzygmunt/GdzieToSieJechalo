namespace Transit.Domain.Entities.Personel;

public class BadanieLekarskie
{
    public int IdBadania { get; private set; }
    public int IdKierowcy { get; private set; }
    public DateOnly DataBadania { get; private set; }
    public DateOnly DataWaznosci { get; private set; }
    public string Wynik { get; private set; } = default!;
    public string Lekarz { get; private set; } = default!;

    public Kierowca Kierowca { get; private set; } = default!;

    private BadanieLekarskie() { }

    public static BadanieLekarskie Utworz(int idKierowcy, DateOnly dataBadania, DateOnly dataWaznosci, string wynik, string lekarz)
    {
        if (dataWaznosci <= dataBadania)
            throw new Exceptions.DomainException("Data ważności badania musi być późniejsza niż data badania.");
        var dozwoloneWyniki = new[] { Constants.WynikBadania.Pozytywny, Constants.WynikBadania.Negatywny };
        if (!dozwoloneWyniki.Contains(wynik))
            throw new Exceptions.DomainException($"Nieznany wynik badania: {wynik}.");

        return new BadanieLekarskie
        {
            IdKierowcy = idKierowcy,
            DataBadania = dataBadania,
            DataWaznosci = dataWaznosci,
            Wynik = wynik,
            Lekarz = lekarz
        };
    }

    public bool JestWazne(DateOnly naDate) =>
        DataWaznosci >= naDate && Wynik == Constants.WynikBadania.Pozytywny;
}
