namespace Transit.Domain.Entities.Siec;

public class PrzystanekWariantu
{
    public int Id { get; private set; }
    public int IdWariantu { get; private set; }
    public int IdPrzystanku { get; private set; }
    public int Kolejnosc { get; private set; }
    public int OdlegloscOdPoczatkuM { get; private set; }

    public WariantTrasy Wariant { get; private set; } = default!;
    public Przystanek Przystanek { get; private set; } = default!;
    public ICollection<Rozklady.OdjazdPlanowy> Odjazdy { get; private set; } = new List<Rozklady.OdjazdPlanowy>();

    private PrzystanekWariantu() { }

    public static PrzystanekWariantu Utworz(int idWariantu, int idPrzystanku, int kolejnosc, int odlegloscOdPoczatkuM = 0) =>
        new()
        {
            IdWariantu = idWariantu,
            IdPrzystanku = idPrzystanku,
            Kolejnosc = kolejnosc,
            OdlegloscOdPoczatkuM = odlegloscOdPoczatkuM
        };
}
