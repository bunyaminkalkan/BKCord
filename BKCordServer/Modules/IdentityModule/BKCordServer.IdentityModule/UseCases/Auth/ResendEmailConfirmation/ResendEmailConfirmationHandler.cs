using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using BKCordServer.IdentityModule.Domain.Entities;
using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.ResendEmailConfirmation;
public sealed class ResendEmailConfirmationHandler : IRequestHandler<ResendEmailConfirmationCommand>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly IMailService _mailService;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public ResendEmailConfirmationHandler(
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

    public async Task Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken)
            ?? throw new NotFoundException($"User not found with {request.Email} email address");

        if (user.EmailConfirmed)
            throw new BadRequestException("Email address is already confirmed.");

        // Rate limiting - son 5 dakikada token gönderilmiş mi?
        var recentToken = await _dbContext.EmailConfirmationTokens
            .Where(t => t.UserId == user.Id && t.CreatedAt > DateTime.UtcNow.AddMinutes(-5))
            .FirstOrDefaultAsync(cancellationToken);

        if (recentToken != null)
            throw new BadRequestException("Please wait at least 5 minutes before requesting another confirmation email.");

        // Önceki tokenları geçersiz kıl ve yeni token oluştur
        await SendEmailConfirmationAsync(user, cancellationToken);
    }

    private async Task SendEmailConfirmationAsync(Domain.Entities.User user, CancellationToken cancellationToken)
    {
        // Önceki tokenları geçersiz kıl
        var existingTokens = await _dbContext.EmailConfirmationTokens
            .Where(t => t.UserId == user.Id && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var token in existingTokens)
        {
            token.IsUsed = true;
            token.UsedAt = DateTime.UtcNow;
        }

        // Yeni token oluştur
        var plainToken = _tokenService.GenerateToken();
        var hashedToken = _tokenService.HashToken(plainToken);

        var confirmationToken = new EmailConfirmationToken
        {
            UserId = user.Id,
            Token = hashedToken,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            IsUsed = false
        };

        _dbContext.EmailConfirmationTokens.Add(confirmationToken);

        var frontendBaseUrl = _configuration["Frontend:BaseUrl"] ?? "https://yourdomain.com";
        var confirmationLink = $"{frontendBaseUrl}/confirm-email?token={plainToken}&id={confirmationToken.Id}";

        string subject = "BKCord - Confirm Your Email Address";
        string body = $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
        </head>
        <body>
            <p>Hello {user.UserName},</p>
            <p>We have received your email confirmation request. To activate your account, please click the link below:</p>
            <a href=""{confirmationLink}"" 
               style=""background-color: #007bff; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px; display: inline-block;"">
                Confirm My Email Address
            </a>
            <p><strong>This link will expire in 24 hours.</strong></p>
            <p>The BKCord Team</p>
        </body>
        </html>";

        await _dbContext.SaveChangesAsync(cancellationToken);
        await _mailService.SendAsync(user.Email, subject, body, cancellationToken);
    }
}
