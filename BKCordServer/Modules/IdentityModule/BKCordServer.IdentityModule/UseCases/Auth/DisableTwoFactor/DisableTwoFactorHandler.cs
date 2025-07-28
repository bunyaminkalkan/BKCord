using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.DisableTwoFactor;
public class DisableTwoFactorHandler : IRequestHandler<DisableTwoFactorCommand>
{
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly ITwoFactorAuthService _twoFactorAuthService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMailService _mailService;
    private readonly IConfiguration _configuration;

    public DisableTwoFactorHandler(
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

    public async Task Handle(DisableTwoFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
            ?? throw new BadRequestException("User not found");

        var success = await _twoFactorAuthService.DisableTwoFactorAsync(user);

        if (!success)
            throw new BadRequestException("Operation failed. Please try again.");

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
}
