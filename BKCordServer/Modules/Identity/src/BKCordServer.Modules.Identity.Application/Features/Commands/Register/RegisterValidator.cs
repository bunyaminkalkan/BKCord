using FluentValidation;

namespace BKCordServer.Modules.Identity.Application.Features.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        // Name - opsiyonel ama girilirse en az 2 karakter
        RuleFor(x => x.Name)
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        // Middlename - opsiyonel ama girilirse en az 2 karakter
        RuleFor(x => x.Middlename)
            .MinimumLength(2).WithMessage("Middle name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Middle name cannot exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Middlename));

        // Surname - opsiyonel ama girilirse en az 2 karakter
        RuleFor(x => x.Surname)
            .MinimumLength(2).WithMessage("Surname must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Surname cannot exceed 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Surname));

        // UserName - zorunlu (Identity default: letters and digits, no special chars except -)
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 character long.")
            .MaximumLength(256).WithMessage("Username cannot exceed 256 characters.")
            .Matches("^[a-zA-Z0-9@.-]+$").WithMessage("Username can only contain letters, numbers, '@', '.' and '-' characters.");

        // Email - zorunlu ve geçerli format (Identity default: max 256 chars)
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please enter a valid email address.")
            .MaximumLength(256).WithMessage("Email cannot exceed 256 characters.");

        // Password - ASP.NET Core Identity default kısıtları
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.")
            .Must(ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
            .Must(ContainUppercase).WithMessage("Password must contain at least one uppercase letter.")
            .Must(ContainDigit).WithMessage("Password must contain at least one digit.")
            .Must(ContainNonAlphanumeric).WithMessage("Password must contain at least one non-alphanumeric character.");

        // ConfirmPassword - zorunlu ve Password ile eşleşmeli
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Password confirmation is required.")
            .Equal(x => x.Password).WithMessage("Password and confirmation password do not match.");
    }

    // Password validation helper methods (Identity default requirements)
    private static bool ContainLowercase(string password)
    {
        return password.Any(char.IsLower);
    }

    private static bool ContainUppercase(string password)
    {
        return password.Any(char.IsUpper);
    }

    private static bool ContainDigit(string password)
    {
        return password.Any(char.IsDigit);
    }

    private static bool ContainNonAlphanumeric(string password)
    {
        return password.Any(c => !char.IsLetterOrDigit(c));
    }
}
