using FluentValidation;

namespace BKCordServer.IdentityModule.UseCases.Auth.ForgotPassword;
public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordValidator()
    {
        RuleFor(fp => fp.Email)
            .NotEmpty().WithMessage("Email field cannot be empty")
            .EmailAddress().WithMessage("Please enter a valid email address");
    }
}
