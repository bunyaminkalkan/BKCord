using FluentValidation;
using System.Text.RegularExpressions;

namespace BKCordServer.ServerModule.UseCases.Role.CreateRole;
public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleValidator()
    {
        RuleFor(r => r.ServerId)
            .NotEmpty()
            .WithMessage("Server ID is required.");

        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Role name is required.")
            .MaximumLength(100)
            .WithMessage("Role name cannot exceed 100 characters.");

        RuleFor(r => r.Color)
            .MaximumLength(7)
            .WithMessage("Color code cannot exceed 7 characters.")
            .Must(BeValidHexColor)
            .When(r => !string.IsNullOrEmpty(r.Color))
            .WithMessage("Color must be a valid hex color code (e.g., #FFFFFF).");

        RuleFor(r => r.Hierarchy)
            .NotNull()
            .WithMessage("Hierarchy is required.")
            .GreaterThanOrEqualTo((short)0)
            .WithMessage("Hierarchy must be greater than or equal to 0.");
    }

    private bool BeValidHexColor(string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return false; // Null/empty is allowed since it's optional

        // Check if it's a valid hex color format (#RRGGBB or #RGB)
        var hexColorPattern = @"^#([A-Fa-f0-9]{6})$";
        return Regex.IsMatch(color, hexColorPattern);
    }
}
