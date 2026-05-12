namespace Transit.Domain.Entities.Slowniki;

public class UprawnienieKategorii
{
    public int IdUprawnienia { get; private set; }
    public string Kategoria { get; private set; } = default!;
    public string Opis { get; private set; } = default!;

    public ICollection<Personel.UprawnienieKierowcy> UprawnieniKierowcow { get; private set; } = new List<Personel.UprawnienieKierowcy>();

    private UprawnienieKategorii() { }

    public static UprawnienieKategorii Utworz(string kategoria, string opis) =>
        new() { Kategoria = kategoria, Opis = opis };
}
