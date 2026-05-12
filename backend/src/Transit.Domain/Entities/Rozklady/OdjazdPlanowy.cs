namespace Transit.Domain.Entities.Rozklady;

public class OdjazdPlanowy
{
    public int IdOdjazdu { get; private set; }
    public int IdRozkladu { get; private set; }
    public int IdPrzystankuWariantu { get; private set; }
    public TimeOnly PlanowaGodzinaOdjazdu { get; private set; }
    public short OffsetDni { get; private set; }
    public int NrKursu { get; private set; }

    public RozkladJazdy Rozklad { get; private set; } = default!;
    public Siec.PrzystanekWariantu PrzystanekWariantu { get; private set; } = default!;
    public ICollection<DziennikPrzejazdu> Dzienniki { get; private set; } = new List<DziennikPrzejazdu>();

    private OdjazdPlanowy() { }

    public static OdjazdPlanowy Utworz(int idRozkladu, int idPrzystankuWariantu, TimeOnly godzinaOdjazdu, int nrKursu, short offsetDni = 0) =>
        new()
        {
            IdRozkladu = idRozkladu,
            IdPrzystankuWariantu = idPrzystankuWariantu,
            PlanowaGodzinaOdjazdu = godzinaOdjazdu,
            NrKursu = nrKursu,
            OffsetDni = offsetDni
        };
}
