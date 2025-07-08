using FluentValidation;

namespace BKCordServer.IdentityModule.UseCases.Auth.RefreshToken;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("{PropertyName} is required.")
            .Length(44)
            .WithMessage("{PropertyName} must be at 44 characters long.");
    }
}
