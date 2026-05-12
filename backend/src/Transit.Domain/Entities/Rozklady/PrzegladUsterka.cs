using Transit.Domain.Constants;

namespace Transit.Domain.Entities.Rozklady;

public class PrzegladUsterka
{
    public int IdPrzegladu { get; private set; }
    public int IdPojazdu { get; private set; }
    public int IdTypuPrzegladu { get; private set; }
    public DateOnly DataPrzegladu { get; private set; }
    public string Wynik { get; private set; } = default!;
    public string? Uwagi { get; private set; }
    public string Warsztat { get; private set; } = default!;

    public Pojazdy.Pojazd Pojazd { get; private set; } = default!;
    public Slowniki.TypPrzegladu TypPrzegladu { get; private set; } = default!;

    private PrzegladUsterka() { }

    public static PrzegladUsterka Utworz(int idPojazdu, int idTypuPrzegladu, DateOnly dataPrzegladu, string wynik, string warsztat, string? uwagi = null)
    {
        var dozwoloneWyniki = new[] { WynikPrzegladu.Pozytywny, WynikPrzegladu.Negatywny, WynikPrzegladu.WymagujeNaprawy };
        if (!dozwoloneWyniki.Contains(wynik))
            throw new Exceptions.DomainException($"Nieznany wynik przeglądu: {wynik}.");

        return new PrzegladUsterka
        {
            IdPojazdu = idPojazdu,
            IdTypuPrzegladu = idTypuPrzegladu,
            DataPrzegladu = dataPrzegladu,
            Wynik = wynik,
            Warsztat = warsztat,
            Uwagi = uwagi
        };
    }
}
