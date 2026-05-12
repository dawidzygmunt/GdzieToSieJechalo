namespace Transit.Domain.Entities.Pasazerowie;

public class Mandat
{
    public int IdMandatu { get; private set; }
    public int IdKontroli { get; private set; }
    public int? IdPasazera { get; private set; }
    public string? NrDokumentuPasazera { get; private set; }
    public decimal Kwota { get; private set; }
    public string Powod { get; private set; } = default!;
    public DateOnly? DataPlatnosci { get; private set; }
    public bool Oplacony { get; private set; }

    public KontrolaWPojedzie Kontrola { get; private set; } = default!;
    public Pasazer? Pasazer { get; private set; }

    private Mandat() { }

    public static Mandat Utworz(int idKontroli, decimal kwota, string powod, int? idPasazera = null, string? nrDokumentu = null)
    {
        if (idPasazera is null && string.IsNullOrWhiteSpace(nrDokumentu))
            throw new Exceptions.DomainException("Mandat musi być powiązany z pasażerem lub zawierać numer dokumentu.");
        if (kwota <= 0)
            throw new Exceptions.DomainException("Kwota mandatu musi być dodatnia.");

        return new Mandat
        {
            IdKontroli = idKontroli,
            IdPasazera = idPasazera,
            NrDokumentuPasazera = nrDokumentu,
            Kwota = kwota,
            Powod = powod
        };
    }

    public void ZapiszPlatnosc(DateOnly dataPlatnosci)
    {
        DataPlatnosci = dataPlatnosci;
        Oplacony = true;
    }
}
