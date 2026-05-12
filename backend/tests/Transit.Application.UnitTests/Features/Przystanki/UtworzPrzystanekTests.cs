using FluentAssertions;
using Transit.Application.Common.Exceptions;
using Transit.Application.Features.Przystanki.Commands;
using Transit.Application.UnitTests.Helpers;
using Transit.Domain.Entities.Slowniki;

namespace Transit.Application.UnitTests.Features.Przystanki;

public class UtworzPrzystanekTests
{
    [Fact]
    public async Task Handler_tworzy_przystanek_gdy_dzielnica_istnieje()
    {
        using var db = InMemoryDbContextFactory.Create();
        var dzielnica = Dzielnica.Utworz("Śródmieście");
        db.Dzielnice.Add(dzielnica);
        await db.SaveChangesAsync();

        var handler = new UtworzPrzystanekHandler(db);
        var cmd = new UtworzPrzystanekCommand(dzielnica.IdDzielnicy, "Centrum", "ul. Główna 1", "naziemny", false);

        var id = await handler.Handle(cmd, CancellationToken.None);

        id.Should().BeGreaterThan(0);
        db.Przystanki.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handler_rzuca_NotFoundException_gdy_dzielnica_nie_istnieje()
    {
        using var db = InMemoryDbContextFactory.Create();
        var handler = new UtworzPrzystanekHandler(db);
        var cmd = new UtworzPrzystanekCommand(999, "Centrum", "ul. Główna 1", "naziemny", false);

        var act = async () => await handler.Handle(cmd, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
