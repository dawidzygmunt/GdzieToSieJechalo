using Transit.Application.Features.Polaczenia.Queries.WyszukajPolaczenia;

namespace Transit.Application.Abstractions.Routing;

public interface IWyszukiwaczPolaczen
{
    Task<IReadOnlyList<PolaczenieDto>> WyszukajAsync(
        int idPrzystankuZ,
        int idPrzystankuDo,
        DateOnly data,
        TimeOnly czas,
        int maxWynikow,
        CancellationToken ct = default);
}
