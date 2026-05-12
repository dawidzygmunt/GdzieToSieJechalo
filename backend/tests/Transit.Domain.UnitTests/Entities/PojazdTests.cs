using FluentAssertions;
using Transit.Domain.Entities.Pojazdy;
using Transit.Domain.Exceptions;

namespace Transit.Domain.UnitTests.Entities;

public class PojazdTests
{
    private static readonly int IdModelu = 1;
    private static readonly DateOnly DataZakupu = new DateOnly(2022, 1, 1);

    [Fact]
    public void Utworz_prawidlowe_dane_tworzy_pojazd()
    {
        var p = Pojazd.Utworz(IdModelu, "1001", "WH1234567890ABCDE", 2022, DataZakupu);
        p.NumerBoczny.Should().Be("1001");
        p.Aktywny.Should().BeTrue();
    }

    [Fact]
    public void Utworz_rok_w_przyszlosci_rzuca_wyjatek()
    {
        var przyszlyRok = DateTime.UtcNow.Year + 1;
        var act = () => Pojazd.Utworz(IdModelu, "1001", "WH1234567890ABCDE", przyszlyRok, DataZakupu);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Deaktywuj_ustawia_Aktywny_na_false()
    {
        var p = Pojazd.Utworz(IdModelu, "1001", "WH1234567890ABCDE", 2022, DataZakupu);
        p.Deaktywuj();
        p.Aktywny.Should().BeFalse();
    }

    [Fact]
    public void Aktywuj_po_deaktywacji_przywraca_aktywnosc()
    {
        var p = Pojazd.Utworz(IdModelu, "1001", "WH1234567890ABCDE", 2022, DataZakupu);
        p.Deaktywuj();
        p.Aktywuj();
        p.Aktywny.Should().BeTrue();
    }
}
