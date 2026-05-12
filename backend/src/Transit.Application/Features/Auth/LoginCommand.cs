using FluentValidation;
using MediatR;
using Transit.Application.Abstractions.Identity;

namespace Transit.Application.Features.Auth;

public record LoginCommand(string Email, string Haslo) : IRequest<AuthResponse>;

public record AuthResponse(string AccessToken, string RefreshToken);

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Haslo).NotEmpty();
    }
}

public class LoginHandler(IIdentityService identityService) : IRequestHandler<LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var (success, accessToken, refreshToken, errors) = await identityService.ZalogujAsync(request.Email, request.Haslo);
        if (!success)
            throw new Common.Exceptions.ValidationException(
                errors.Select(e => new FluentValidation.Results.ValidationFailure("Credentials", e)));
        return new AuthResponse(accessToken, refreshToken);
    }
}
