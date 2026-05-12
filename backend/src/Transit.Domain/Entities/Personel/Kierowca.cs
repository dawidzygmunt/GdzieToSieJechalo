namespace Transit.Domain.Entities.Personel;

public class Kierowca
{
    public int IdKierowcy { get; private set; }
    public string Imie { get; private set; } = default!;
    public string Nazwisko { get; private set; } = default!;
    public string NrPracownika { get; private set; } = default!;
    public DateOnly DataZatrudnienia { get; private set; }
    public bool Aktywny { get; private set; } = true;

    public ICollection<UprawnienieKierowcy> Uprawnienia { get; private set; } = new List<UprawnienieKierowcy>();
    public ICollection<BadanieLekarskie> Badania { get; private set; } = new List<BadanieLekarskie>();
    public ICollection<Rozklady.RealizacjaKursu> Realizacje { get; private set; } = new List<Rozklady.RealizacjaKursu>();
    public ICollection<Rozklady.GrafikPracy> Grafiki { get; private set; } = new List<Rozklady.GrafikPracy>();

    private Kierowca() { }

    public static Kierowca Utworz(string imie, string nazwisko, string nrPracownika, DateOnly dataZatrudnienia) =>
        new()
        {
            Imie = imie,
            Nazwisko = nazwisko,
            NrPracownika = nrPracownika,
            DataZatrudnienia = dataZatrudnienia
        };

    public string PelneNazwisko => $"{Imie} {Nazwisko}";

    public bool MaWazneUprawnienie(int idUprawnienia, DateOnly naDate) =>
        Uprawnienia.Any(u => u.IdUprawnienia == idUprawnienia && u.DataWaznosci >= naDate);

    public bool MaWazneUprawnienieKategorii(string kategoria, DateOnly naDate) =>
        Uprawnienia.Any(u => u.UprawnienieKategorii?.Kategoria == kategoria && u.DataWaznosci >= naDate);

    public bool MaWaznieBadanie(DateOnly naDate) =>
        Badania.Any(b => b.DataWaznosci >= naDate && b.Wynik == Constants.WynikBadania.Pozytywny);

    public void Deaktywuj() => Aktywny = false;
}
