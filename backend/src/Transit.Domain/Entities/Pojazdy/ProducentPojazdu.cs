namespace Transit.Domain.Entities.Pojazdy;

public class ProducentPojazdu
{
    public int IdProducenta { get; private set; }
    public string Nazwa { get; private set; } = default!;

    public ICollection<ModelPojazdu> Modele { get; private set; } = new List<ModelPojazdu>();

    private ProducentPojazdu() { }

    public static ProducentPojazdu Utworz(string nazwa) => new() { Nazwa = nazwa };
}
