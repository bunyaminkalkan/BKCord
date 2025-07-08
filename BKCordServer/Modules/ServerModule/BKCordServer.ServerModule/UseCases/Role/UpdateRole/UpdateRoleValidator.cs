using BKCordServer.ServerModule.Domain.Enums;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BKCordServer.ServerModule.UseCases.Role.UpdateRole;
public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleValidator()
    {
        RuleFor(r => r.RoleId)
            .NotEmpty()
            .WithMessage("Role ID is required.");

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

        RuleFor(r => r.RolePermissions)
            .NotNull()
            .WithMessage("Role permissions list is required.");

        RuleFor(r => r.RolePermissions)
            .Must(HaveUniquePermissions)
            .When(r => r.RolePermissions != null && r.RolePermissions.Count > 0)
            .WithMessage("Duplicate permissions are not allowed.");

        RuleForEach(r => r.RolePermissions)
            .IsInEnum()
            .WithMessage("Invalid permission type.");
    }

    private bool BeValidHexColor(string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return false;

        var hexColorPattern = @"^#([A-Fa-f0-9]{6})$";
        return Regex.IsMatch(color, hexColorPattern);
    }

    private bool HaveUniquePermissions(List<RolePermission> permissions)
    {
        var uniquePermissions = permissions.Distinct().Count();
        return uniquePermissions == permissions.Count;
    }
}
