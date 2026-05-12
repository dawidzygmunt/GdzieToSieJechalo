namespace Transit.Domain.Entities.Personel;

public class Kontroler
{
    public int IdKontrolera { get; private set; }
    public string Imie { get; private set; } = default!;
    public string Nazwisko { get; private set; } = default!;
    public string NrSluzbowy { get; private set; } = default!;
    public bool Aktywny { get; private set; } = true;

    public ICollection<Pasazerowie.KontrolaWPojedzie> Kontrole { get; private set; } = new List<Pasazerowie.KontrolaWPojedzie>();

    private Kontroler() { }

    public static Kontroler Utworz(string imie, string nazwisko, string nrSluzbowy) =>
        new() { Imie = imie, Nazwisko = nazwisko, NrSluzbowy = nrSluzbowy };

    public string PelneNazwisko => $"{Imie} {Nazwisko}";
    public void Deaktywuj() => Aktywny = false;
}
