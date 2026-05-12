using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Application.Common.Exceptions;
using Transit.Domain.Entities.Rozklady;

namespace Transit.Application.Features.RealizacjeKursow.Commands;

public record UtworzRealizacjeCommand(int IdWariantu, DateOnly DataKursu, int NrKursu) : IRequest<int>, ICommand<int>;

public class UtworzRealizacjeValidator : AbstractValidator<UtworzRealizacjeCommand>
{
    public UtworzRealizacjeValidator()
    {
        RuleFor(x => x.IdWariantu).GreaterThan(0);
        RuleFor(x => x.NrKursu).GreaterThan(0);
    }
}

public class UtworzRealizacjeHandler(IApplicationDbContext db) : IRequestHandler<UtworzRealizacjeCommand, int>
{
    public async Task<int> Handle(UtworzRealizacjeCommand request, CancellationToken ct)
    {
        _ = await db.WariantyTras.FirstOrDefaultAsync(w => w.IdWariantu == request.IdWariantu && w.Aktywny, ct)
            ?? throw new NotFoundException("WariantTrasy", request.IdWariantu);

        var realizacja = RealizacjaKursu.Utworz(request.IdWariantu, request.DataKursu, request.NrKursu);
        db.RealizacjeKursow.Add(realizacja);
        await db.SaveChangesAsync(ct);
        return realizacja.IdRealizacji;
    }
}
