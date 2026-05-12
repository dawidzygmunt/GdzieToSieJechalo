using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Application.Common.Exceptions;
using Transit.Domain.Entities.Rozklady;

namespace Transit.Application.Features.Przeglady.Commands;

public record DodajPrzegladCommand(int IdPojazdu, int IdTypuPrzegladu, DateOnly DataPrzegladu, string Wynik, string Warsztat, string? Uwagi)
    : IRequest<int>, ICommand<int>;

public class DodajPrzegladValidator : AbstractValidator<DodajPrzegladCommand>
{
    public DodajPrzegladValidator()
    {
        RuleFor(x => x.IdPojazdu).GreaterThan(0);
        RuleFor(x => x.IdTypuPrzegladu).GreaterThan(0);
        RuleFor(x => x.Wynik).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Warsztat).NotEmpty().MaximumLength(100);
    }
}

public class DodajPrzegladHandler(IApplicationDbContext db) : IRequestHandler<DodajPrzegladCommand, int>
{
    public async Task<int> Handle(DodajPrzegladCommand request, CancellationToken ct)
    {
        _ = await db.Pojazdy.FirstOrDefaultAsync(p => p.IdPojazdu == request.IdPojazdu, ct)
            ?? throw new NotFoundException("Pojazd", request.IdPojazdu);
        _ = await db.TypyPrzegladu.FirstOrDefaultAsync(t => t.IdTypuPrzegladu == request.IdTypuPrzegladu, ct)
            ?? throw new NotFoundException("TypPrzegladu", request.IdTypuPrzegladu);

        var przeglad = PrzegladUsterka.Utworz(
            request.IdPojazdu, request.IdTypuPrzegladu, request.DataPrzegladu, request.Wynik, request.Warsztat, request.Uwagi);
        db.PrzegladyUsterki.Add(przeglad);
        await db.SaveChangesAsync(ct);
        return przeglad.IdPrzegladu;
    }
}
