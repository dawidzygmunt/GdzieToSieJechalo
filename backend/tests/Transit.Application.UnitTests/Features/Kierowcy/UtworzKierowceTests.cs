using FluentAssertions;
using Transit.Application.Features.Kierowcy.Commands;
using Transit.Application.UnitTests.Helpers;

namespace Transit.Application.UnitTests.Features.Kierowcy;

public class UtworzKierowceTests
{
    [Fact]
    public async Task Handler_tworzy_kierowce()
    {
        using var db = InMemoryDbContextFactory.Create();
        var handler = new UtworzKierowceHandler(db);
        var cmd = new UtworzKierowceCommand("Jan", "Kowalski", "KIE001", new DateOnly(2020, 1, 1));

        var id = await handler.Handle(cmd, CancellationToken.None);

        id.Should().BeGreaterThan(0);
        db.Kierowcy.Should().HaveCount(1);
        db.Kierowcy.First().NrPracownika.Should().Be("KIE001");
    }

    [Fact]
    public async Task Validator_odrzuca_pustego_kierowce()
    {
        var validator = new UtworzKierowceValidator();
        var cmd = new UtworzKierowceCommand("", "", "", new DateOnly(2020, 1, 1));
        var result = validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
