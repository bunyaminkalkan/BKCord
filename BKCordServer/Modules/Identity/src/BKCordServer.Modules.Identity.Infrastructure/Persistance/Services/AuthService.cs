using BKCordServer.Modules.Identity.Application.Services;
using BKCordServer.Modules.Identity.Domain.Entities;
using BKCordServer.Modules.Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace BKCordServer.Modules.Identity.Infrastructure.Persistance.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;

    public AuthService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task RegisterAsync(string email, string userName, string password)
    {
        var now = DateTime.UtcNow;

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = userName,
            Email = email,
            AvatarUrl = "",
            Status = UserStatus.Active,
            IsPrivateAccount = false,
            EmailConfirmed = false,
            CreatedAt = now,
            UpdatedAt = now,
            DeletedAt = now,
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Kayıt başarısız: {errors}");
        }
    }
}
