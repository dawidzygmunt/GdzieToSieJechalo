namespace Transit.Domain.Entities.Rozklady;

public class GrafikPracy
{
    public int IdGrafiku { get; private set; }
    public int IdKierowcy { get; private set; }
    public int IdPojazdu { get; private set; }
    public DateOnly Data { get; private set; }
    public TimeOnly GodzinaOd { get; private set; }
    public TimeOnly GodzinaDo { get; private set; }
    public short OffsetDni { get; private set; }

    public Personel.Kierowca Kierowca { get; private set; } = default!;
    public Pojazdy.Pojazd Pojazd { get; private set; } = default!;

    private GrafikPracy() { }

    public static GrafikPracy Utworz(int idKierowcy, int idPojazdu, DateOnly data, TimeOnly godzinaOd, TimeOnly godzinaDo)
    {
        if (godzinaDo <= godzinaOd)
            throw new Exceptions.DomainException("Godzina końca zmiany musi być późniejsza niż godzina początku.");
        return new GrafikPracy
        {
            IdKierowcy = idKierowcy,
            IdPojazdu = idPojazdu,
            Data = data,
            GodzinaOd = godzinaOd,
            GodzinaDo = godzinaDo
        };
    }
}
