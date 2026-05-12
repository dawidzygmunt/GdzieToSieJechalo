namespace Transit.Domain.Entities.Siec;

public class Linia
{
    public int IdLinii { get; private set; }
    public string NumerLinii { get; private set; } = default!;
    public string TypLinii { get; private set; } = default!;
    public string? Opis { get; private set; }
    public bool Aktywna { get; private set; } = true;

    public ICollection<WariantTrasy> Warianty { get; private set; } = new List<WariantTrasy>();

    private Linia() { }

    public static Linia Utworz(string numerLinii, string typLinii, string? opis = null) =>
        new() { NumerLinii = numerLinii, TypLinii = typLinii, Opis = opis };

    public void Deaktywuj() => Aktywna = false;
    public void Aktywuj() => Aktywna = true;
}
