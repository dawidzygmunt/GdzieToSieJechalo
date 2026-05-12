using Transit.Domain.Constants;

namespace Transit.Domain.Entities.Pojazdy;

public class ModelPojazdu
{
    public int IdModelu { get; private set; }
    public int IdProducenta { get; private set; }
    public string NazwaModelu { get; private set; } = default!;
    public string TypPojazdu { get; private set; } = default!;
    public int LiczbaMiejsc { get; private set; }

    public ProducentPojazdu Producent { get; private set; } = default!;
    public ICollection<Pojazd> Pojazdy { get; private set; } = new List<Pojazd>();

    private ModelPojazdu() { }

    public static ModelPojazdu Utworz(int idProducenta, string nazwaModelu, string typPojazdu, int liczbaMiejsc)
    {
        var dozwoloneTypy = new[] { Constants.TypPojazdu.Autobus, Constants.TypPojazdu.Tramwaj, Constants.TypPojazdu.Trolejbus };
        if (!dozwoloneTypy.AsEnumerable().Contains(typPojazdu))
            throw new Exceptions.DomainException($"Nieznany typ pojazdu: {typPojazdu}.");
        if (liczbaMiejsc <= 0)
            throw new Exceptions.DomainException("Liczba miejsc musi być dodatnia.");

        return new ModelPojazdu
        {
            IdProducenta = idProducenta,
            NazwaModelu = nazwaModelu,
            TypPojazdu = typPojazdu,
            LiczbaMiejsc = liczbaMiejsc
        };
    }
}
