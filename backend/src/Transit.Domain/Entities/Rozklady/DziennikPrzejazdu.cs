namespace Transit.Domain.Entities.Rozklady;

public class DziennikPrzejazdu
{
    public int IdWpisu { get; private set; }
    public int IdRealizacji { get; private set; }
    public int IdOdjazdu { get; private set; }
    public DateTime? RzeczywiistyOdjazd { get; private set; }
    public DateTime? RzeczywiistyPrzyjazd { get; private set; }
    public int? OpoznienieMin { get; private set; }
    public short OffsetDni { get; private set; }

    public RealizacjaKursu Realizacja { get; private set; } = default!;
    public OdjazdPlanowy Odjazd { get; private set; } = default!;

    private DziennikPrzejazdu() { }

    public static DziennikPrzejazdu Utworz(int idRealizacji, int idOdjazdu) =>
        new() { IdRealizacji = idRealizacji, IdOdjazdu = idOdjazdu };

    public void ZapiszOdjazd(DateTime rzeczywistyOdjazd, int opoznienieMin)
    {
        RzeczywiistyOdjazd = rzeczywistyOdjazd;
        OpoznienieMin = opoznienieMin;
    }

    public void ZapiszPrzyjazd(DateTime rzeczywistyPrzyjazd)
    {
        RzeczywiistyPrzyjazd = rzeczywistyPrzyjazd;
    }
}
