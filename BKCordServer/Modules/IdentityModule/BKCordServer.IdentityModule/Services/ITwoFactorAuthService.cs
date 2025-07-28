using BKCordServer.IdentityModule.Domain.Entities;

namespace BKCordServer.IdentityModule.Services;
public interface ITwoFactorAuthService
{
    Task<string> SendTwoFactorCodeAsync(User user);
    Task<bool> EnableTwoFactorAsync(User user, string verificationCode);
    Task<bool> DisableTwoFactorAsync(User user);
    Task<string[]> GenerateRecoveryCodesAsync(User user);
    Task<bool> VerifyTwoFactorTokenAsync(User user, string token);
    Task<bool> VerifyLoginTokenAsync(User user, string token);
}
