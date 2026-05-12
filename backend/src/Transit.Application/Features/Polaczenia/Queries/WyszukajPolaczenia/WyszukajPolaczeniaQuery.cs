using MediatR;

namespace Transit.Application.Features.Polaczenia.Queries.WyszukajPolaczenia;

public record WyszukajPolaczeniaQuery(
    int PrzystanekZ,
    int PrzystanekDo,
    DateOnly Data,
    TimeOnly Czas,
    int MaxWynikow = 10
) : IRequest<IReadOnlyList<PolaczenieDto>>;
