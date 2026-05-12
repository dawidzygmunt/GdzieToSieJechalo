using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Application.Common.Exceptions;
using Transit.Domain.Entities.Pojazdy;

namespace Transit.Application.Features.Pojazdy.Commands;

public record UtworzPojazdCommand(int IdModelu, string NumerBoczny, string Vin, int RokProdukcji, DateOnly DataZakupu)
    : IRequest<int>, ICommand<int>;

public class UtworzPojazdValidator : AbstractValidator<UtworzPojazdCommand>
{
    public UtworzPojazdValidator()
    {
        RuleFor(x => x.IdModelu).GreaterThan(0);
        RuleFor(x => x.NumerBoczny).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Vin).NotEmpty().Length(17);
        RuleFor(x => x.RokProdukcji).InclusiveBetween(1950, DateTime.UtcNow.Year);
    }
}

public class UtworzPojazdHandler(IApplicationDbContext db) : IRequestHandler<UtworzPojazdCommand, int>
{
    public async Task<int> Handle(UtworzPojazdCommand request, CancellationToken ct)
    {
        _ = await db.ModelePojazdow.FirstOrDefaultAsync(m => m.IdModelu == request.IdModelu, ct)
            ?? throw new NotFoundException("ModelPojazdu", request.IdModelu);

        var pojazd = Pojazd.Utworz(request.IdModelu, request.NumerBoczny, request.Vin, request.RokProdukcji, request.DataZakupu);
        db.Pojazdy.Add(pojazd);
        await db.SaveChangesAsync(ct);
        return pojazd.IdPojazdu;
    }
}
