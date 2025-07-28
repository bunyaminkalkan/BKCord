using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using BKCordServer.IdentityModule.Domain.Entities;
using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.ForgotPassword;
public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly IMailService _mailService;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public ForgotPasswordHandler(
        AppIdentityDbContext dbContext,
        IMailService mailService,
        ITokenService tokenService,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mailService = mailService;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email)
            ?? throw new NotFoundException($"User not found with {request.Email} email address");

        var existingTokens = await _dbContext.ForgotPasswordTokens
            .Where(t => t.UserId == user.Id && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var token in existingTokens)
        {
            token.IsUsed = true;
            token.UsedAt = DateTime.UtcNow;
        }

        var plainToken = _tokenService.GenerateToken();
        var hashedToken = _tokenService.HashToken(plainToken);

        var forgotPasswordToken = new ForgotPasswordToken
        {
            UserId = user.Id,
            Token = hashedToken,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(15), // 15 dakika geçerli
            IsUsed = false
        };

        _dbContext.ForgotPasswordTokens.Add(forgotPasswordToken);

        var frontendBaseUrl = _configuration["Frontend:BaseUrl"] ?? "https://yourdomain.com";
        var resetLink = $"{frontendBaseUrl}/reset-password?token={plainToken}&id={forgotPasswordToken.Id}";

        string to = user.Email;
        string subject = "BKCord Password Reset Request";
        string body = $@"
        <!DOCTYPE html>
        <html lang=""tr"">
        <head>
            <meta charset=""UTF-8"">
        </head>
        <body>
            <p>Hello, {user.UserName}</p>
            <p>A password reset request has been made for your account.</p>
            <p>Click on the link below to continue:</p>
            <a href=""{resetLink}"" target=""_blank"">Reset Password</a>
            <p><strong>This link will expire after 15 minutes.</strong></p>
            <p>If you did not make this request, you can ignore this email.</p>
            <p>BKCord Team</p>
        </body>
        </html>";

        await _dbContext.SaveChangesAsync(cancellationToken);
        await _mailService.SendAsync(user.Email, subject, body);
    }
}
