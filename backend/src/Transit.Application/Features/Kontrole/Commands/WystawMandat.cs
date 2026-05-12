using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Application.Common.Exceptions;
using Transit.Domain.Entities.Pasazerowie;

namespace Transit.Application.Features.Kontrole.Commands;

public record WystawMandatCommand(int IdKontroli, decimal Kwota, string Powod, int? IdPasazera, string? NrDokumentu)
    : IRequest<int>, ICommand<int>;

public class WystawMandatValidator : AbstractValidator<WystawMandatCommand>
{
    public WystawMandatValidator()
    {
        RuleFor(x => x.IdKontroli).GreaterThan(0);
        RuleFor(x => x.Kwota).GreaterThan(0).WithMessage("Kwota mandatu musi być dodatnia.");
        RuleFor(x => x.Powod).NotEmpty().MaximumLength(200);
        RuleFor(x => x).Must(x => x.IdPasazera.HasValue || !string.IsNullOrWhiteSpace(x.NrDokumentu))
            .WithMessage("Podaj ID pasażera lub numer dokumentu.");
    }
}

public class WystawMandatHandler(IApplicationDbContext db) : IRequestHandler<WystawMandatCommand, int>
{
    public async Task<int> Handle(WystawMandatCommand request, CancellationToken ct)
    {
        _ = await db.KontrolWPojazdach.FirstOrDefaultAsync(k => k.IdKontroli == request.IdKontroli, ct)
            ?? throw new NotFoundException("KontrolaWPojedzie", request.IdKontroli);

        var mandat = Mandat.Utworz(request.IdKontroli, request.Kwota, request.Powod, request.IdPasazera, request.NrDokumentu);
        db.Mandaty.Add(mandat);
        await db.SaveChangesAsync(ct);
        return mandat.IdMandatu;
    }
}
