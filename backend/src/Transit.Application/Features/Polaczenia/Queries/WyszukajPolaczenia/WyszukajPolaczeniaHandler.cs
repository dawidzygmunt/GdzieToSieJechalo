using MediatR;
using Transit.Application.Abstractions.Routing;

namespace Transit.Application.Features.Polaczenia.Queries.WyszukajPolaczenia;

public class WyszukajPolaczeniaHandler(IWyszukiwaczPolaczen wyszukiwacz)
    : IRequestHandler<WyszukajPolaczeniaQuery, IReadOnlyList<PolaczenieDto>>
{
    public Task<IReadOnlyList<PolaczenieDto>> Handle(WyszukajPolaczeniaQuery request, CancellationToken ct) =>
        wyszukiwacz.WyszukajAsync(
            request.PrzystanekZ,
            request.PrzystanekDo,
            request.Data,
            request.Czas,
            request.MaxWynikow,
            ct);
}
