using FluentValidation;
using MediatR;
using Transit.Application.Abstractions.Identity;

namespace Transit.Application.Features.Auth;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponse>;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public class RefreshTokenHandler(IIdentityService identityService) : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var (success, accessToken, refreshToken, errors) = await identityService.OdswiezTokenAsync(request.RefreshToken);
        if (!success)
            throw new Common.Exceptions.ValidationException(
                errors.Select(e => new FluentValidation.Results.ValidationFailure("RefreshToken", e)));
        return new AuthResponse(accessToken, refreshToken);
    }
}
