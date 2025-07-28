using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.SendTwoFactorVerificationCode;
public class SendTwoFactorVerificationCodeHandler : IRequestHandler<SendTwoFactorVerificationCodeCommand>
{
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly ITwoFactorAuthService _twoFactorAuthService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMailService _mailService;
    private readonly IConfiguration _configuration;

    public SendTwoFactorVerificationCodeHandler(
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

    public async Task Handle(SendTwoFactorVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
            ?? throw new BadRequestException("User not found");

        var token = await _twoFactorAuthService.SendTwoFactorCodeAsync(user);

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
    }
}
