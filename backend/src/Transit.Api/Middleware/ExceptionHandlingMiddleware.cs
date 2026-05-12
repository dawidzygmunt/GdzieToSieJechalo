using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Transit.Application.Common.Exceptions;
using Transit.Domain.Exceptions;
using ValidationException = Transit.Application.Common.Exceptions.ValidationException;

namespace Transit.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await next(ctx);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Nieobsłużony wyjątek: {Message}", ex.Message);
            await HandleAsync(ctx, ex);
        }
    }

    private static async Task HandleAsync(HttpContext ctx, Exception ex)
    {
        var (statusCode, problem) = ex switch
        {
            ValidationException ve => (400, new ValidationProblemDetails(
                ve.Errors.ToDictionary(kv => kv.Key, kv => kv.Value))
            {
                Status = 400, Title = "Błąd walidacji", Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            }),
            NotFoundException => (404, new ProblemDetails
            {
                Status = 404, Title = "Nie znaleziono", Detail = ex.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            }),
            ForbiddenException => (403, new ProblemDetails
            {
                Status = 403, Title = "Brak dostępu", Detail = ex.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
            }),
            DomainException => (422, new ProblemDetails
            {
                Status = 422, Title = "Naruszenie reguły domenowej", Detail = ex.Message,
                Type = "https://tools.ietf.org/html/rfc4918#section-11.2"
            }),
            _ => (500, new ProblemDetails
            {
                Status = 500, Title = "Błąd serwera",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            })
        };

        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode = statusCode;
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
