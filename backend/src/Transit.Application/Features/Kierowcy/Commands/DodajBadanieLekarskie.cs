using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Application.Common.Exceptions;
using Transit.Domain.Entities.Personel;

namespace Transit.Application.Features.Kierowcy.Commands;

public record DodajBadanieLekarsieCommand(int IdKierowcy, DateOnly DataBadania, DateOnly DataWaznosci, string Wynik, string Lekarz)
    : IRequest<int>, ICommand<int>;

public class DodajBadanieValidator : AbstractValidator<DodajBadanieLekarsieCommand>
{
    public DodajBadanieValidator()
    {
        RuleFor(x => x.IdKierowcy).GreaterThan(0);
        RuleFor(x => x.Lekarz).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Wynik).NotEmpty().MaximumLength(10);
        RuleFor(x => x.DataWaznosci).GreaterThan(x => x.DataBadania);
    }
}

public class DodajBadanieHandler(IApplicationDbContext db) : IRequestHandler<DodajBadanieLekarsieCommand, int>
{
    public async Task<int> Handle(DodajBadanieLekarsieCommand request, CancellationToken ct)
    {
        _ = await db.Kierowcy.FirstOrDefaultAsync(k => k.IdKierowcy == request.IdKierowcy, ct)
            ?? throw new NotFoundException("Kierowca", request.IdKierowcy);

        var badanie = BadanieLekarskie.Utworz(
            request.IdKierowcy, request.DataBadania, request.DataWaznosci, request.Wynik, request.Lekarz);
        db.BadaniaLekarskie.Add(badanie);
        await db.SaveChangesAsync(ct);
        return badanie.IdBadania;
    }
}
