using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Application.Common.Exceptions;

namespace Transit.Application.Features.RealizacjeKursow.Commands;

public record PrzypiszKierowcePojazdCommand(int IdRealizacji, int IdKierowcy, int IdPojazdu) : IRequest, ICommand;

public class PrzypiszKierowcePojazdValidator : AbstractValidator<PrzypiszKierowcePojazdCommand>
{
    public PrzypiszKierowcePojazdValidator()
    {
        RuleFor(x => x.IdRealizacji).GreaterThan(0);
        RuleFor(x => x.IdKierowcy).GreaterThan(0);
        RuleFor(x => x.IdPojazdu).GreaterThan(0);
    }
}

public class PrzypiszKierowcePojazdHandler(IApplicationDbContext db) : IRequestHandler<PrzypiszKierowcePojazdCommand>
{
    public async Task Handle(PrzypiszKierowcePojazdCommand request, CancellationToken ct)
    {
        var realizacja = await db.RealizacjeKursow
            .Include(r => r.Wariant).ThenInclude(w => w.Linia)
            .FirstOrDefaultAsync(r => r.IdRealizacji == request.IdRealizacji, ct)
            ?? throw new NotFoundException("RealizacjaKursu", request.IdRealizacji);

        var kierowca = await db.Kierowcy
            .Include(k => k.Badania)
            .Include(k => k.Uprawnienia).ThenInclude(u => u.UprawnienieKategorii)
            .FirstOrDefaultAsync(k => k.IdKierowcy == request.IdKierowcy && k.Aktywny, ct)
            ?? throw new NotFoundException("Kierowca", request.IdKierowcy);

        var dzisiaj = realizacja.DataKursu;
        if (!kierowca.MaWaznieBadanie(dzisiaj))
            throw new Domain.Exceptions.DomainException("Kierowca nie ma ważnego badania lekarskiego na dzień kursu.");

        _ = await db.Pojazdy.FirstOrDefaultAsync(p => p.IdPojazdu == request.IdPojazdu && p.Aktywny, ct)
            ?? throw new NotFoundException("Pojazd", request.IdPojazdu);

        realizacja.PrzypiszKierowce(request.IdKierowcy);
        realizacja.PrzypiszPojazd(request.IdPojazdu);
        await db.SaveChangesAsync(ct);
    }
}
