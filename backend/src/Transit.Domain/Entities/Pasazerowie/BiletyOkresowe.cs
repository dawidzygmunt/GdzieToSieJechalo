namespace Transit.Domain.Entities.Pasazerowie;

public class BiletyOkresowe
{
    public int IdBiletu { get; private set; }
    public int IdPasazera { get; private set; }
    public int IdKategorii { get; private set; }
    public DateOnly DataOd { get; private set; }
    public DateOnly DataDo { get; private set; }
    public decimal Cena { get; private set; }
    public bool Aktywny { get; private set; } = true;

    public Pasazer Pasazer { get; private set; } = default!;
    public Slowniki.KategoriaOplaty Kategoria { get; private set; } = default!;

    private BiletyOkresowe() { }

    public static BiletyOkresowe Utworz(int idPasazera, int idKategorii, DateOnly dataOd, DateOnly dataDo, decimal cena)
    {
        if (dataDo <= dataOd)
            throw new Exceptions.DomainException("Data końca ważności biletu musi być późniejsza niż data początku.");
        if (cena < 0)
            throw new Exceptions.DomainException("Cena biletu nie może być ujemna.");

        return new BiletyOkresowe
        {
            IdPasazera = idPasazera,
            IdKategorii = idKategorii,
            DataOd = dataOd,
            DataDo = dataDo,
            Cena = cena
        };
    }

    public bool JestWazny(DateOnly naDate) => Aktywny && naDate >= DataOd && naDate <= DataDo;
    public void Deaktywuj() => Aktywny = false;
}
