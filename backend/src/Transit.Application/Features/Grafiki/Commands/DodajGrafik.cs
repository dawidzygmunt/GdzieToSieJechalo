using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Application.Common.Exceptions;
using Transit.Domain.Entities.Rozklady;

namespace Transit.Application.Features.Grafiki.Commands;

public record DodajGrafikCommand(int IdKierowcy, int IdPojazdu, DateOnly Data, TimeOnly GodzinaOd, TimeOnly GodzinaDo)
    : IRequest<int>, ICommand<int>;

public class DodajGrafikValidator : AbstractValidator<DodajGrafikCommand>
{
    public DodajGrafikValidator()
    {
        RuleFor(x => x.IdKierowcy).GreaterThan(0);
        RuleFor(x => x.IdPojazdu).GreaterThan(0);
        RuleFor(x => x.GodzinaDo).GreaterThan(x => x.GodzinaOd);
    }
}

public class DodajGrafikHandler(IApplicationDbContext db) : IRequestHandler<DodajGrafikCommand, int>
{
    public async Task<int> Handle(DodajGrafikCommand request, CancellationToken ct)
    {
        _ = await db.Kierowcy.FirstOrDefaultAsync(k => k.IdKierowcy == request.IdKierowcy && k.Aktywny, ct)
            ?? throw new NotFoundException("Kierowca", request.IdKierowcy);
        _ = await db.Pojazdy.FirstOrDefaultAsync(p => p.IdPojazdu == request.IdPojazdu && p.Aktywny, ct)
            ?? throw new NotFoundException("Pojazd", request.IdPojazdu);

        var grafik = GrafikPracy.Utworz(request.IdKierowcy, request.IdPojazdu, request.Data, request.GodzinaOd, request.GodzinaDo);
        db.GrafikiPracy.Add(grafik);
        await db.SaveChangesAsync(ct);
        return grafik.IdGrafiku;
    }
}
