using FluentValidation;

namespace BKCordServer.IdentityModule.UseCases.Auth.ConfirmEmail;
public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailValidator()
    {
        RuleFor(x => x.TokenId)
            .NotEmpty().WithMessage("Token ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Token ID cannot be empty.");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Confirmation token is required.")
            .MinimumLength(10).WithMessage("Invalid token format.")
            .MaximumLength(500).WithMessage("Invalid token format.");
    }
}
