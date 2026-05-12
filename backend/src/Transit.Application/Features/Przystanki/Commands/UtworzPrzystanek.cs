using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Application.Common.Exceptions;
using Transit.Domain.Entities.Siec;

namespace Transit.Application.Features.Przystanki.Commands;

public record UtworzPrzystanekCommand(int IdDzielnicy, string Nazwa, string Ulica, string Typ, bool Wiata)
    : IRequest<int>, ICommand<int>;

public class UtworzPrzystanekValidator : AbstractValidator<UtworzPrzystanekCommand>
{
    public UtworzPrzystanekValidator()
    {
        RuleFor(x => x.IdDzielnicy).GreaterThan(0);
        RuleFor(x => x.Nazwa).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Ulica).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Typ).NotEmpty().MaximumLength(20);
    }
}

public class UtworzPrzystanekHandler(IApplicationDbContext db)
    : IRequestHandler<UtworzPrzystanekCommand, int>
{
    public async Task<int> Handle(UtworzPrzystanekCommand request, CancellationToken ct)
    {
        _ = await db.Dzielnice.FirstOrDefaultAsync(d => d.IdDzielnicy == request.IdDzielnicy, ct)
            ?? throw new NotFoundException("Dzielnica", request.IdDzielnicy);

        var przystanek = Przystanek.Utworz(request.IdDzielnicy, request.Nazwa, request.Ulica, request.Typ, request.Wiata);
        db.Przystanki.Add(przystanek);
        await db.SaveChangesAsync(ct);
        return przystanek.IdPrzystanku;
    }
}
