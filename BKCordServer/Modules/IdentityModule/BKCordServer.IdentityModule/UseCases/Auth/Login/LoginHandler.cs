using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using BKCordServer.IdentityModule.DTOs;
using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.Login;

public class LoginHandler : IRequestHandler<LoginCommand, JwtResponse>
{
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly IJwtService _jwtService;
    private readonly ITwoFactorAuthService _twoFactorAuthService;
    private readonly AppIdentityDbContext _appIdentityDbContext;

    public LoginHandler(
        UserManager<Domain.Entities.User> userManager,
        IJwtService jwtService,
        ITwoFactorAuthService twoFactorAuthService,
        AppIdentityDbContext appIdentityDbContext)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _twoFactorAuthService = twoFactorAuthService;
        _appIdentityDbContext = appIdentityDbContext;
    }

    public async Task<JwtResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new BadRequestException("Invalid email or password");

        if (!user.EmailConfirmed)
            throw new BadRequestException("Your user email address is not verified. You must verify your email address to log in.");

        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isValidPassword)
            throw new BadRequestException("Invalid email or password");

        // 2FA kontrolü
        if (user.TwoFactorEnabled)
        {
            // Eğer 2FA kodu sağlanmamışsa, kullanıcıya kod gönder ve 2FA gerekli yanıtı döndür
            if (string.IsNullOrEmpty(request.TwoFactorCode))
            {
                await _twoFactorAuthService.SendTwoFactorCodeAsync(user);

                return new JwtResponse
                {
                    RequiresTwoFactor = true,
                    Message = "Two-factor authentication required. A verification code has been sent to your email."
                };
            }

            // 2FA kodu sağlanmışsa doğrula
            var isValidTwoFactorCode = await _twoFactorAuthService.VerifyLoginTokenAsync(user, request.TwoFactorCode);

            if (!isValidTwoFactorCode)
            {
                // Recovery code kontrolü
                var isValidRecoveryCode = await _userManager.RedeemTwoFactorRecoveryCodeAsync(user, request.TwoFactorCode);

                if (!isValidRecoveryCode.Succeeded)
                {
                    throw new BadRequestException("Invalid two-factor authentication code");
                }
            }
        }

        // Token oluştur ve refresh token kaydet
        var tokens = await _jwtService.CreateTokenAsync(user);

        var refreshToken = new Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            Token = tokens.RefreshToken
        };

        _appIdentityDbContext.RefreshTokens.Add(refreshToken);
        await _appIdentityDbContext.SaveChangesAsync(cancellationToken);

        return tokens;
    }
}