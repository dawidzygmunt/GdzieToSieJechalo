using FluentAssertions;
using Transit.Domain.Constants;
using Transit.Domain.Entities.Personel;

namespace Transit.Domain.UnitTests.Entities;

public class KierowcaTests
{
    [Fact]
    public void Nowy_kierowca_jest_aktywny()
    {
        var k = Kierowca.Utworz("Jan", "Kowalski", "K001", new DateOnly(2020, 1, 1));
        k.Aktywny.Should().BeTrue();
    }

    [Fact]
    public void Deaktywuj_ustawia_Aktywny_na_false()
    {
        var k = Kierowca.Utworz("Jan", "Kowalski", "K001", new DateOnly(2020, 1, 1));
        k.Deaktywuj();
        k.Aktywny.Should().BeFalse();
    }

    [Fact]
    public void MaWaznieBadanie_gdy_brak_badan_zwraca_false()
    {
        var k = Kierowca.Utworz("Jan", "Kowalski", "K001", new DateOnly(2020, 1, 1));
        k.MaWaznieBadanie(DateOnly.FromDateTime(DateTime.UtcNow)).Should().BeFalse();
    }

    [Fact]
    public void BadanieLekarskie_z_negatywnym_wynikiem_nie_spelnia_warunku_waznosci()
    {
        var dzisiaj = DateOnly.FromDateTime(DateTime.UtcNow);
        var b = BadanieLekarskie.Utworz(1, dzisiaj.AddDays(-30), dzisiaj.AddDays(330), WynikBadania.Negatywny, "Dr Kowalski");
        b.JestWazne(dzisiaj).Should().BeFalse();
    }
}
