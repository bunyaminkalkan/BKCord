using BKCordServer.IdentityModule.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BKCordServer.IdentityModule.Services;
public class TwoFactorAuthService : ITwoFactorAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IMailService _mailService;
    private readonly IConfiguration _configuration;

    public TwoFactorAuthService(UserManager<User> userManager, IMailService mailService, IConfiguration configuration)
    {
        _userManager = userManager;
        _mailService = mailService;
        _configuration = configuration;
    }

    public async Task<bool> SendTwoFactorCodeAsync(User user)
    {
        try
        {
            // E-posta için 2FA token oluştur
            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            var appName = _configuration["AppName"] ?? "BKCord";
            var subject = $"{appName} - Two-Factor Authentication Code";

            var body = $@"
                <h2>Two-Factor Authentication</h2>
                <p>Hello {user.Name},</p>
                <p>Please use the following verification code to sign in to your account:</p>
                <h1 style='color: #007bff; font-family: monospace; letter-spacing: 3px;'>{token}</h1>
                <p>This code will expire in 5 minutes.</p>
                <p>If you didn't request this code, please check your account security.</p>
                <br>
                <p>Best regards,<br>{appName} Team</p>
            ";

            await _mailService.SendAsync(user.Email, subject, body);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> EnableTwoFactorAsync(User user, string verificationCode)
    {
        // E-posta token'ını doğrula
        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", verificationCode);

        if (isValid)
        {
            await _userManager.SetTwoFactorEnabledAsync(user, true);

            // Kullanıcıya 2FA aktif edildi bildirimi gönder
            await SendTwoFactorEnabledNotificationAsync(user);
            return true;
        }

        return false;
    }

    public async Task<bool> DisableTwoFactorAsync(User user)
    {
        var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
        if (result.Succeeded)
        {
            // Kullanıcıya 2FA devre dışı bırakıldı bildirimi gönder
            await SendTwoFactorDisabledNotificationAsync(user);
        }
        return result.Succeeded;
    }

    public async Task<string[]> GenerateRecoveryCodesAsync(User user)
    {
        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        // Recovery code'ları e-posta ile gönder
        await SendRecoveryCodesAsync(user, recoveryCodes.ToArray());

        return recoveryCodes.ToArray();
    }

    public async Task<bool> VerifyTwoFactorTokenAsync(User user, string token)
    {
        // Setup sırasında token doğrulama
        return await _userManager.VerifyTwoFactorTokenAsync(user, "Email", token);
    }

    public async Task<bool> VerifyLoginTokenAsync(User user, string token)
    {
        // Login sırasında token doğrulama
        return await _userManager.VerifyTwoFactorTokenAsync(user, "Email", token);
    }

    private async Task SendTwoFactorEnabledNotificationAsync(User user)
    {
        var appName = _configuration["AppName"] ?? "BKCord";
        var subject = $"{appName} - Two-Factor Authentication Enabled";

        var body = $@"
            <h2>Two-Factor Authentication Enabled</h2>
            <p>Hello {user.Name},</p>
            <p>Two-factor authentication has been successfully enabled for your account.</p>
            <p>From now on, you will need to enter a verification code sent to your email address when signing in.</p>
            <p>Your account security has been enhanced! 🛡️</p>
            <br>
            <p>Best regards,<br>{appName} Team</p>
        ";

        await _mailService.SendAsync(user.Email, subject, body);
    }

    private async Task SendTwoFactorDisabledNotificationAsync(User user)
    {
        var appName = _configuration["AppName"] ?? "BKCord";
        var subject = $"{appName} - Two-Factor Authentication Disabled";

        var body = $@"
            <h2>Two-Factor Authentication Disabled</h2>
            <p>Hello {user.Name},</p>
            <p>Two-factor authentication has been disabled for your account.</p>
            <p>If you didn't make this change, please check your account security immediately.</p>
            <br>
            <p>Best regards,<br>{appName} Team</p>
        ";

        await _mailService.SendAsync(user.Email, subject, body);
    }

    private async Task SendRecoveryCodesAsync(User user, string[] recoveryCodes)
    {
        var appName = _configuration["AppName"] ?? "BKCord";
        var subject = $"{appName} - Your Recovery Codes";

        var codesHtml = string.Join("<br>", recoveryCodes.Select(code =>
            $"<code style='background: #f8f9fa; padding: 2px 6px; border-radius: 3px; font-family: monospace;'>{code}</code>"));

        var body = $@"
            <h2>Your Recovery Codes</h2>
            <p>Hello {user.Name},</p>
            <p>Here are your two-factor authentication recovery codes:</p>
            <div style='margin: 20px 0; padding: 15px; background: #f8f9fa; border-radius: 5px;'>
                {codesHtml}
            </div>
            <p><strong>IMPORTANT:</strong></p>
            <ul>
                <li>Store these codes in a safe place</li>
                <li>Each code can only be used once</li>
                <li>You can use these codes if you lose access to your email</li>
                <li>Make sure to save these codes before deleting this email</li>
            </ul>
            <br>
            <p>Best regards,<br>{appName} Team</p>
        ";

        await _mailService.SendAsync(user.Email, subject, body);
    }
}