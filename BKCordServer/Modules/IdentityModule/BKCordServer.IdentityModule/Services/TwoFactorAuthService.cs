using BKCordServer.IdentityModule.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.Services;
public class TwoFactorAuthService : ITwoFactorAuthService
{
    private readonly UserManager<User> _userManager;

    public TwoFactorAuthService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> SendTwoFactorCodeAsync(User user)
    {
        try
        {
            // E-posta için 2FA token oluştur
            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            return token;
        }
        catch
        {
            throw new InternalServerErrorException("Failed to create verification code. Please try again.");
        }
    }

    public async Task<bool> EnableTwoFactorAsync(User user, string verificationCode)
    {
        // E-posta token'ını doğrula
        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", verificationCode);

        if (isValid)
        {
            await _userManager.SetTwoFactorEnabledAsync(user, true);
            return true;
        }

        return false;
    }

    public async Task<bool> DisableTwoFactorAsync(User user) =>
        (await _userManager.SetTwoFactorEnabledAsync(user, false)).Succeeded;

    public async Task<string[]> GenerateRecoveryCodesAsync(User user) =>
        (await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10)).ToArray();

    public async Task<bool> VerifyLoginTokenAsync(User user, string token) =>
        await _userManager.VerifyTwoFactorTokenAsync(user, "Email", token);
}