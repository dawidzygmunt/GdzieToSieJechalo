using Transit.Domain.Constants;

namespace Transit.Domain.Entities.Rozklady;

public class RealizacjaKursu
{
    public int IdRealizacji { get; private set; }
    public int IdWariantu { get; private set; }
    public int? IdKierowcy { get; private set; }
    public int? IdPojazdu { get; private set; }
    public DateOnly DataKursu { get; private set; }
    public int NrKursu { get; private set; }
    public string StatusKursu { get; private set; } = Constants.StatusKursu.Zaplanowany;

    public Siec.WariantTrasy Wariant { get; private set; } = default!;
    public Personel.Kierowca? Kierowca { get; private set; }
    public Pojazdy.Pojazd? Pojazd { get; private set; }
    public ICollection<DziennikPrzejazdu> Dzienniki { get; private set; } = new List<DziennikPrzejazdu>();
    public ICollection<Pasazerowie.KontrolaWPojedzie> Kontrole { get; private set; } = new List<Pasazerowie.KontrolaWPojedzie>();

    private RealizacjaKursu() { }

    public static RealizacjaKursu Utworz(int idWariantu, DateOnly dataKursu, int nrKursu) =>
        new() { IdWariantu = idWariantu, DataKursu = dataKursu, NrKursu = nrKursu };

    public void PrzypiszKierowce(int idKierowcy) => IdKierowcy = idKierowcy;
    public void PrzypiszPojazd(int idPojazdu) => IdPojazdu = idPojazdu;

    public void ZmienStatus(string nowyStatus)
    {
        if (nowyStatus == Constants.StatusKursu.Zrealizowany && (IdKierowcy is null || IdPojazdu is null))
            throw new Exceptions.DomainException("Kurs może być oznaczony jako zrealizowany tylko gdy przypisano kierowcę i pojazd.");
        StatusKursu = nowyStatus;
    }
}
