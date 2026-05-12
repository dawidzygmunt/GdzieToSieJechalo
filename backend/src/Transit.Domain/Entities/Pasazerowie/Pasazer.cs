namespace Transit.Domain.Entities.Pasazerowie;

public class Pasazer
{
    public int IdPasazera { get; private set; }
    public string Imie { get; private set; } = default!;
    public string Nazwisko { get; private set; } = default!;
    public string? Pesel { get; private set; }
    public string? Email { get; private set; }

    public ICollection<BiletyOkresowe> Bilety { get; private set; } = new List<BiletyOkresowe>();
    public ICollection<Mandat> Mandaty { get; private set; } = new List<Mandat>();

    private Pasazer() { }

    public static Pasazer Utworz(string imie, string nazwisko, string? pesel = null, string? email = null)
    {
        if (pesel is not null && pesel.Length != 11)
            throw new Exceptions.DomainException("PESEL musi mieć dokładnie 11 znaków.");
        return new Pasazer { Imie = imie, Nazwisko = nazwisko, Pesel = pesel, Email = email };
    }

    public string PelneNazwisko => $"{Imie} {Nazwisko}";
}
