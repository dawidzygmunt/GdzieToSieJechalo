using FluentValidation;
using MediatR;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Domain.Entities.Pasazerowie;

namespace Transit.Application.Features.Pasazerowie.Commands;

public record UtworzPasazeraCommand(string Imie, string Nazwisko, string? Pesel, string? Email) : IRequest<int>, ICommand<int>;

public class UtworzPasazeraValidator : AbstractValidator<UtworzPasazeraCommand>
{
    public UtworzPasazeraValidator()
    {
        RuleFor(x => x.Imie).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Nazwisko).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Pesel).Length(11).When(x => x.Pesel is not null);
        RuleFor(x => x.Email).EmailAddress().When(x => x.Email is not null);
    }
}

public class UtworzPasazeraHandler(IApplicationDbContext db) : IRequestHandler<UtworzPasazeraCommand, int>
{
    public async Task<int> Handle(UtworzPasazeraCommand request, CancellationToken ct)
    {
        var pasazer = Pasazer.Utworz(request.Imie, request.Nazwisko, request.Pesel, request.Email);
        db.Pasazerowie.Add(pasazer);
        await db.SaveChangesAsync(ct);
        return pasazer.IdPasazera;
    }
}
