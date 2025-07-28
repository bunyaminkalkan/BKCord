using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.EnableTwoFactor;
public class EnableTwoFactorHandler : IRequestHandler<EnableTwoFactorCommand, string[]>
{
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly ITwoFactorAuthService _twoFactorAuthService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMailService _mailService;
    private readonly IConfiguration _configuration;

    public EnableTwoFactorHandler(
        UserManager<Domain.Entities.User> userManager,
        ITwoFactorAuthService twoFactorAuthService,
        IHttpContextAccessor httpContextAccessor,
        IMailService mailService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _twoFactorAuthService = twoFactorAuthService;
        _httpContextAccessor = httpContextAccessor;
        _mailService = mailService;
        _configuration = configuration;
    }

    public async Task<string[]> Handle(EnableTwoFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
            ?? throw new BadRequestException("User not found");

        var success = await _twoFactorAuthService.EnableTwoFactorAsync(user, request.VerificationCode);
        if (!success)
            throw new BadRequestException("Invalid verification code. Please try again.");

        var recoveryCodes = await _twoFactorAuthService.GenerateRecoveryCodesAsync(user);
        var appName = _configuration["AppName"] ?? "BKCord";

        // Recovery codes HTML formatı
        var codesHtml = string.Join("<br>", recoveryCodes.Select(code =>
            $"<code style='background: #f8f9fa; padding: 2px 6px; border-radius: 3px; font-family: monospace;'>{code}</code>"));

        var subject = $"{appName} - Two-Factor Authentication Enabled";
        var body = $@"
        <h2>Two-Factor Authentication Enabled</h2>
        <p>Hello {user.Name},</p>
        <p>Two-factor authentication has been successfully enabled for your account.</p>
        <p>From now on, you will need to enter a verification code sent to your email address when signing in.</p>
        <p>Your account security has been enhanced! 🛡️</p>
        
        <hr style='margin: 30px 0; border: none; border-top: 1px solid #e0e0e0;'>
        
        <h2>Your Recovery Codes</h2>
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
        <p>Best regards,<br>{appName} Team</p>";

        await _mailService.SendAsync(user.Email, subject, body);

        return recoveryCodes;
    }
}
