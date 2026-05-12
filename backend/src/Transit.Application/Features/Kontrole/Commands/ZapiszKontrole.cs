using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Application.Common.Exceptions;
using Transit.Domain.Entities.Pasazerowie;

namespace Transit.Application.Features.Kontrole.Commands;

public record ZapiszKontroleCommand(int IdKontrolera, int IdRealizacji, DateTime DataGodzina, string Wynik) : IRequest<int>, ICommand<int>;

public class ZapiszKontroleValidator : AbstractValidator<ZapiszKontroleCommand>
{
    public ZapiszKontroleValidator()
    {
        RuleFor(x => x.IdKontrolera).GreaterThan(0);
        RuleFor(x => x.IdRealizacji).GreaterThan(0);
        RuleFor(x => x.Wynik).NotEmpty().MaximumLength(30);
    }
}

public class ZapiszKontroleHandler(IApplicationDbContext db) : IRequestHandler<ZapiszKontroleCommand, int>
{
    public async Task<int> Handle(ZapiszKontroleCommand request, CancellationToken ct)
    {
        _ = await db.Kontrolerzy.FirstOrDefaultAsync(k => k.IdKontrolera == request.IdKontrolera && k.Aktywny, ct)
            ?? throw new NotFoundException("Kontroler", request.IdKontrolera);
        _ = await db.RealizacjeKursow.FirstOrDefaultAsync(r => r.IdRealizacji == request.IdRealizacji, ct)
            ?? throw new NotFoundException("RealizacjaKursu", request.IdRealizacji);

        var kontrola = KontrolaWPojedzie.Utworz(request.IdKontrolera, request.IdRealizacji, request.DataGodzina, request.Wynik);
        db.KontrolWPojazdach.Add(kontrola);
        await db.SaveChangesAsync(ct);
        return kontrola.IdKontroli;
    }
}
