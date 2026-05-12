using FluentAssertions;
using Transit.Domain.Entities.Pasazerowie;
using Transit.Domain.Exceptions;

namespace Transit.Domain.UnitTests.Entities;

public class MandatTests
{
    [Fact]
    public void Mandat_bez_pasazera_i_numeru_dokumentu_rzuca_wyjatek()
    {
        var act = () => Mandat.Utworz(1, 100m, "Brak biletu", null, null);
        act.Should().Throw<DomainException>()
            .WithMessage("*pasażerem lub zawierać numer dokumentu*");
    }

    [Fact]
    public void Mandat_ujemna_kwota_rzuca_wyjatek()
    {
        var act = () => Mandat.Utworz(1, -50m, "Powód", idPasazera: 1);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Mandat_z_pasazerem_tworzy_sie_poprawnie()
    {
        var m = Mandat.Utworz(1, 150m, "Brak biletu", idPasazera: 5);
        m.Kwota.Should().Be(150m);
        m.Oplacony.Should().BeFalse();
    }

    [Fact]
    public void ZapiszPlatnosc_ustawia_Oplacony_na_true()
    {
        var m = Mandat.Utworz(1, 150m, "Brak biletu", nrDokumentu: "ABC123");
        m.ZapiszPlatnosc(DateOnly.FromDateTime(DateTime.UtcNow));
        m.Oplacony.Should().BeTrue();
    }
}
