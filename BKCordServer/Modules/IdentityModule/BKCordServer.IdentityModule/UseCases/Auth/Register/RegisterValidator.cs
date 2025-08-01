﻿using BKCordServer.IdentityModule.UseCases.Auth.ValidationHelpers;
using FluentValidation;

namespace BKCordServer.IdentityModule.UseCases.Auth.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        // Name - opsiyonel ama girilirse en az 2 karakter
        RuleFor(x => x.Name)
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        // Middlename - opsiyonel ama girilirse en az 2 karakter
        RuleFor(x => x.Middlename)
            .MinimumLength(2).WithMessage("Middle name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Middle name cannot exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Middlename));

        // Surname - opsiyonel ama girilirse en az 2 karakter
        RuleFor(x => x.Surname)
            .MinimumLength(2).WithMessage("Surname must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Surname cannot exceed 100 characters.")
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
            .Must(PasswordValidationHelper.ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
            .Must(PasswordValidationHelper.ContainUppercase).WithMessage("Password must contain at least one uppercase letter.")
            .Must(PasswordValidationHelper.ContainDigit).WithMessage("Password must contain at least one digit.")
            .Must(PasswordValidationHelper.ContainNonAlphanumeric).WithMessage("Password must contain at least one non-alphanumeric character.");

        // ConfirmPassword - zorunlu ve Password ile eşleşmeli
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Password confirmation is required.")
            .Equal(x => x.Password).WithMessage("Password and confirmation password do not match.");
    }
}
