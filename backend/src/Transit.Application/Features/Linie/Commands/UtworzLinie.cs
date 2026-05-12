using FluentValidation;
using MediatR;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Domain.Entities.Siec;

namespace Transit.Application.Features.Linie.Commands;

public record UtworzLinieCommand(string NumerLinii, string TypLinii, string? Opis) : IRequest<int>, ICommand<int>;

public class UtworzLinieValidator : AbstractValidator<UtworzLinieCommand>
{
    public UtworzLinieValidator()
    {
        RuleFor(x => x.NumerLinii).NotEmpty().MaximumLength(10);
        RuleFor(x => x.TypLinii).NotEmpty().MaximumLength(20);
    }
}

public class UtworzLinieHandler(IApplicationDbContext db) : IRequestHandler<UtworzLinieCommand, int>
{
    public async Task<int> Handle(UtworzLinieCommand request, CancellationToken ct)
    {
        var linia = Linia.Utworz(request.NumerLinii, request.TypLinii, request.Opis);
        db.Linie.Add(linia);
        await db.SaveChangesAsync(ct);
        return linia.IdLinii;
    }
}
