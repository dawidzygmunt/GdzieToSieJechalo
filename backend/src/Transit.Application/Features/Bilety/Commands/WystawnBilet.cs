using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Application.Common.Exceptions;
using Transit.Domain.Entities.Pasazerowie;

namespace Transit.Application.Features.Bilety.Commands;

public record WystawnBiletCommand(int IdPasazera, int IdKategorii, DateOnly DataOd, DateOnly DataDo, decimal Cena)
    : IRequest<int>, ICommand<int>;

public class WystawnBiletValidator : AbstractValidator<WystawnBiletCommand>
{
    public WystawnBiletValidator()
    {
        RuleFor(x => x.IdPasazera).GreaterThan(0);
        RuleFor(x => x.IdKategorii).GreaterThan(0);
        RuleFor(x => x.DataDo).GreaterThan(x => x.DataOd);
        RuleFor(x => x.Cena).GreaterThanOrEqualTo(0);
    }
}

public class WystawnBiletHandler(IApplicationDbContext db) : IRequestHandler<WystawnBiletCommand, int>
{
    public async Task<int> Handle(WystawnBiletCommand request, CancellationToken ct)
    {
        _ = await db.Pasazerowie.FirstOrDefaultAsync(p => p.IdPasazera == request.IdPasazera, ct)
            ?? throw new NotFoundException("Pasazer", request.IdPasazera);
        _ = await db.KategorieOplat.FirstOrDefaultAsync(k => k.IdKategorii == request.IdKategorii, ct)
            ?? throw new NotFoundException("KategoriaOplaty", request.IdKategorii);

        var bilet = BiletyOkresowe.Utworz(request.IdPasazera, request.IdKategorii, request.DataOd, request.DataDo, request.Cena);
        db.BiletyOkresowe.Add(bilet);
        await db.SaveChangesAsync(ct);
        return bilet.IdBiletu;
    }
}
