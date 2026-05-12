using FluentAssertions;
using Transit.Domain.Constants;
using Transit.Domain.Entities.Rozklady;
using Transit.Domain.Exceptions;

namespace Transit.Domain.UnitTests.Entities;

public class RealizacjaKursuTests
{
    private readonly DateOnly _data = new DateOnly(2026, 5, 12);

    [Fact]
    public void Nowa_realizacja_ma_status_zaplanowany()
    {
        var r = RealizacjaKursu.Utworz(1, _data, 1);
        r.StatusKursu.Should().Be(StatusKursu.Zaplanowany);
    }

    [Fact]
    public void ZmienStatus_na_zrealizowany_bez_kierowcy_rzuca_wyjatek()
    {
        var r = RealizacjaKursu.Utworz(1, _data, 1);
        r.PrzypiszPojazd(5);

        var act = () => r.ZmienStatus(StatusKursu.Zrealizowany);
        act.Should().Throw<DomainException>()
            .WithMessage("*kierowcę*");
    }

    [Fact]
    public void ZmienStatus_na_zrealizowany_bez_pojazdu_rzuca_wyjatek()
    {
        var r = RealizacjaKursu.Utworz(1, _data, 1);
        r.PrzypiszKierowce(3);

        var act = () => r.ZmienStatus(StatusKursu.Zrealizowany);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void ZmienStatus_na_zrealizowany_z_kierowca_i_pojazdem_dziala()
    {
        var r = RealizacjaKursu.Utworz(1, _data, 1);
        r.PrzypiszKierowce(3);
        r.PrzypiszPojazd(5);

        r.ZmienStatus(StatusKursu.Zrealizowany);
        r.StatusKursu.Should().Be(StatusKursu.Zrealizowany);
    }
}
