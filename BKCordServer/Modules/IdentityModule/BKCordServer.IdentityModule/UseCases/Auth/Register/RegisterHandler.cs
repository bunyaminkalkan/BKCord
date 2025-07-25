﻿using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using BKCordServer.IdentityModule.Domain.Entities;
using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand>
{
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly AppIdentityDbContext _dbContext;
    private readonly IMailService _mailService;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public RegisterHandler(
        UserManager<Domain.Entities.User> userManager,
        AppIdentityDbContext dbContext,
        IMailService mailService,
        ITokenService tokenService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _mailService = mailService;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Email = request.Email,
            Name = request.Name,
            Middlename = request.Middlename,
            Surname = request.Surname,
            IsPrivateAccount = false,
            EmailConfirmed = false,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"Register failed: {errors}");
        }

        // Email confirmation token oluştur
        await SendEmailConfirmationAsync(user, cancellationToken);
    }

    private async Task SendEmailConfirmationAsync(Domain.Entities.User user, CancellationToken cancellationToken)
    {
        // Önceki kullanılmamış tokenları geçersiz kıl
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
            ExpiresAt = DateTime.UtcNow.AddHours(24), // 24 saat geçerli
            IsUsed = false
        };

        _dbContext.EmailConfirmationTokens.Add(confirmationToken);

        // Confirmation link oluştur
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
            <p>Welcome to BKCord! To activate your account, you need to confirm your email address.</p>
            <p>Please click the button below to confirm your email address:</p>
            <a href=""{confirmationLink}""
               style=""background-color: #007bff; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px; display: inline-block;"">
                Confirm My Email Address
            </a>
            <p><strong>This link will expire in 24 hours.</strong></p>
            <p>If you did not create this account, you can safely ignore this email.</p>
            <p>The BKCord Team</p>
        </body>
        </html>";

        await _dbContext.SaveChangesAsync(cancellationToken);
        await _mailService.SendAsync(user.Email, subject, body, cancellationToken);
    }
}
