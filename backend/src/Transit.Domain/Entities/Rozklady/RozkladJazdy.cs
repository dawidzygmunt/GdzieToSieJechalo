namespace Transit.Domain.Entities.Rozklady;

public class RozkladJazdy
{
    public int IdRozkladu { get; private set; }
    public int IdWariantu { get; private set; }
    public int IdTypuDnia { get; private set; }
    public DateOnly DataWaznosciOd { get; private set; }
    public DateOnly? DataWaznosciDo { get; private set; }
    public bool Aktywny { get; private set; } = true;

    public Siec.WariantTrasy Wariant { get; private set; } = default!;
    public Slowniki.TypDnia TypDnia { get; private set; } = default!;
    public ICollection<OdjazdPlanowy> Odjazdy { get; private set; } = new List<OdjazdPlanowy>();

    private RozkladJazdy() { }

    public static RozkladJazdy Utworz(int idWariantu, int idTypuDnia, DateOnly dataWaznosciOd, DateOnly? dataWaznosciDo = null)
    {
        if (dataWaznosciDo.HasValue && dataWaznosciDo <= dataWaznosciOd)
            throw new Exceptions.DomainException("Data końca ważności rozkładu musi być późniejsza niż data początku.");
        return new RozkladJazdy
        {
            IdWariantu = idWariantu,
            IdTypuDnia = idTypuDnia,
            DataWaznosciOd = dataWaznosciOd,
            DataWaznosciDo = dataWaznosciDo
        };
    }

    public bool ObowiazujeWDniu(DateOnly data) =>
        Aktywny && data >= DataWaznosciOd && (!DataWaznosciDo.HasValue || data <= DataWaznosciDo.Value);
}
