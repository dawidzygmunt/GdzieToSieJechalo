using FluentAssertions;
using Transit.Domain.Exceptions;
using Transit.Domain.ValueObjects;

namespace Transit.Domain.UnitTests.ValueObjects;

public class VinTests
{
    [Theory]
    [InlineData("WH1234567890ABCDE")]
    [InlineData("1HGBH41JXMN109186")]
    [InlineData("JH4KA7650MC002421")]
    public void Vin_prawidlowy_tworzy_value_object(string rawVin)
    {
        var vin = new Vin(rawVin);
        vin.Value.Should().Be(rawVin.ToUpperInvariant());
    }

    [Theory]
    [InlineData("")]
    [InlineData("ZA KROTKI")]
    [InlineData("1234567890ABCDEFG1")]
    [InlineData("WH123456789_ABCDE")]
    public void Vin_nieprawidlowy_rzuca_DomainException(string rawVin)
    {
        var act = () => new Vin(rawVin);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Vin_konwertuje_do_uppercase()
    {
        var vin = new Vin("wh1234567890abcde");
        vin.Value.Should().Be("WH1234567890ABCDE");
    }

    [Fact]
    public void Dwa_Vin_z_tym_samym_numerem_sa_rowne()
    {
        var a = new Vin("WH1234567890ABCDE");
        var b = new Vin("WH1234567890ABCDE");
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }
}
