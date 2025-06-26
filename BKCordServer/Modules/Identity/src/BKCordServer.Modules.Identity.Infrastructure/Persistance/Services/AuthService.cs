using BKCordServer.Modules.Identity.Application.Features.Commands.Register;
using BKCordServer.Modules.Identity.Application.Services;
using BKCordServer.Modules.Identity.Domain.Entities;
using BKCordServer.Modules.Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Shared.Kernel.Exceptions;

namespace BKCordServer.Modules.Identity.Infrastructure.Persistance.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;

    public AuthService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task RegisterAsync(RegisterCommand request)
    {
        var now = DateTime.UtcNow;

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.UserName,
            Email = request.Email,
            Name = request.Name,
            Middlename = request.Middlename,
            Surname = request.Surname,
            AvatarUrl = "",
            Status = UserStatus.Active,
            IsPrivateAccount = false,
            EmailConfirmed = false,
            CreatedAt = now,
            UpdatedAt = now,
            DeletedAt = now,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"Register failed: {errors}");
        }
    }
}
