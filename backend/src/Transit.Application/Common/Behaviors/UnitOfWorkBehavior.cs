using MediatR;
using Transit.Application.Abstractions.Persistence;

namespace Transit.Application.Common.Behaviors;

public interface ICommand { }
public interface ICommand<TResponse> { }

public class UnitOfWorkBehavior<TRequest, TResponse>(IApplicationDbContext db)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var response = await next(ct);

        if (request is ICommand || (request.GetType().GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>))))
        {
            await db.SaveChangesAsync(ct);
        }

        return response;
    }
}
