using BKCordServer.IdentityModule.UseCases.Auth.ValidationHelpers;
using FluentValidation;

namespace BKCordServer.IdentityModule.UseCases.Auth.ResetPassword;
public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.TokenId)
            .NotEmpty().WithMessage("Token ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Token ID cannot be empty.");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Reset token is required.")
            .MinimumLength(10).WithMessage("Invalid token format.")
            .MaximumLength(500).WithMessage("Invalid token format.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New Password is required.")
            .MinimumLength(6).WithMessage("New Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("New Password cannot exceed 100 characters.")
            .Must(PasswordValidationHelper.ContainLowercase).WithMessage("New Password must contain at least one lowercase letter.")
            .Must(PasswordValidationHelper.ContainUppercase).WithMessage("New Password must contain at least one uppercase letter.")
            .Must(PasswordValidationHelper.ContainDigit).WithMessage("New Password must contain at least one digit.")
            .Must(PasswordValidationHelper.ContainNonAlphanumeric).WithMessage("New Password must contain at least one non-alphanumeric character.");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty().WithMessage("New Password confirmation is required.")
            .Equal(x => x.NewPassword).WithMessage("New Password and confirmation password do not match.");
    }
}
