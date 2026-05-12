namespace Transit.Domain.Entities.Personel;

public class UprawnienieKierowcy
{
    public int Id { get; private set; }
    public int IdKierowcy { get; private set; }
    public int IdUprawnienia { get; private set; }
    public DateOnly DataUzyskania { get; private set; }
    public DateOnly DataWaznosci { get; private set; }

    public Kierowca Kierowca { get; private set; } = default!;
    public Slowniki.UprawnienieKategorii UprawnienieKategorii { get; private set; } = default!;

    private UprawnienieKierowcy() { }

    public static UprawnienieKierowcy Utworz(int idKierowcy, int idUprawnienia, DateOnly dataUzyskania, DateOnly dataWaznosci)
    {
        if (dataWaznosci <= dataUzyskania)
            throw new Exceptions.DomainException("Data ważności musi być późniejsza niż data uzyskania.");
        return new UprawnienieKierowcy
        {
            IdKierowcy = idKierowcy,
            IdUprawnienia = idUprawnienia,
            DataUzyskania = dataUzyskania,
            DataWaznosci = dataWaznosci
        };
    }

    public bool JestWazne(DateOnly naDate) => DataWaznosci >= naDate;
}
