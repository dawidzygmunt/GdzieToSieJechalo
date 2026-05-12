using FluentValidation;
using MediatR;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Domain.Entities.Personel;

namespace Transit.Application.Features.Kierowcy.Commands;

public record UtworzKierowceCommand(string Imie, string Nazwisko, string NrPracownika, DateOnly DataZatrudnienia)
    : IRequest<int>, ICommand<int>;

public class UtworzKierowceValidator : AbstractValidator<UtworzKierowceCommand>
{
    public UtworzKierowceValidator()
    {
        RuleFor(x => x.Imie).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Nazwisko).NotEmpty().MaximumLength(50);
        RuleFor(x => x.NrPracownika).NotEmpty().MaximumLength(20);
        RuleFor(x => x.DataZatrudnienia).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
    }
}

public class UtworzKierowceHandler(IApplicationDbContext db) : IRequestHandler<UtworzKierowceCommand, int>
{
    public async Task<int> Handle(UtworzKierowceCommand request, CancellationToken ct)
    {
        var kierowca = Kierowca.Utworz(request.Imie, request.Nazwisko, request.NrPracownika, request.DataZatrudnienia);
        db.Kierowcy.Add(kierowca);
        await db.SaveChangesAsync(ct);
        return kierowca.IdKierowcy;
    }
}
