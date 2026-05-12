using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Common.Behaviors;
using Transit.Application.Common.Exceptions;
using Transit.Domain.Entities.Personel;

namespace Transit.Application.Features.Kierowcy.Commands;

public record NadajUprawnienieCommand(int IdKierowcy, int IdUprawnienia, DateOnly DataUzyskania, DateOnly DataWaznosci)
    : IRequest<int>, ICommand<int>;

public class NadajUprawnienieValidator : AbstractValidator<NadajUprawnienieCommand>
{
    public NadajUprawnienieValidator()
    {
        RuleFor(x => x.IdKierowcy).GreaterThan(0);
        RuleFor(x => x.IdUprawnienia).GreaterThan(0);
        RuleFor(x => x.DataWaznosci).GreaterThan(x => x.DataUzyskania)
            .WithMessage("Data ważności musi być późniejsza niż data uzyskania.");
    }
}

public class NadajUprawnienieHandler(IApplicationDbContext db) : IRequestHandler<NadajUprawnienieCommand, int>
{
    public async Task<int> Handle(NadajUprawnienieCommand request, CancellationToken ct)
    {
        _ = await db.Kierowcy.FirstOrDefaultAsync(k => k.IdKierowcy == request.IdKierowcy, ct)
            ?? throw new NotFoundException("Kierowca", request.IdKierowcy);
        _ = await db.UprawieniaKategorii.FirstOrDefaultAsync(u => u.IdUprawnienia == request.IdUprawnienia, ct)
            ?? throw new NotFoundException("UprawnienieKategorii", request.IdUprawnienia);

        var uprawnienie = UprawnienieKierowcy.Utworz(
            request.IdKierowcy, request.IdUprawnienia, request.DataUzyskania, request.DataWaznosci);
        db.UprawieniaKierowcow.Add(uprawnienie);
        await db.SaveChangesAsync(ct);
        return uprawnienie.Id;
    }
}
