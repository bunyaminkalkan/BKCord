using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.ConfirmEmail;
public sealed class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly ITokenService _tokenService;

    public ConfirmEmailHandler(
        AppIdentityDbContext dbContext,
        UserManager<Domain.Entities.User> userManager,
        ITokenService tokenService)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        // Token'ı veritabanından bul
        var tokenRecord = await _dbContext.EmailConfirmationTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken)
            ?? throw new NotFoundException("Invalid confirmation link.");

        // Token geçerlilik kontrolleri
        if (tokenRecord.IsUsed)
            throw new BadRequestException("This confirmation link has already been used.");

        if (tokenRecord.ExpiresAt < DateTime.UtcNow)
            throw new BadRequestException("The confirmation link has expired.");

        // Token doğrulama
        if (!_tokenService.ValidateToken(request.Token, tokenRecord.Token))
            throw new BadRequestException("Invalid confirmation link.");

        var user = tokenRecord.User;

        // Email zaten onaylanmış mı?
        if (user.EmailConfirmed)
            throw new BadRequestException("This email address is already confirmed.");

        // Email'i onayla
        user.EmailConfirmed = true;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"An error occurred while confirming the email: {errors}");
        }

        // Token'ı kullanılmış olarak işaretle
        tokenRecord.IsUsed = true;
        tokenRecord.UsedAt = DateTime.UtcNow;

        // Kullanıcının diğer aktif email confirmation tokenlarını da geçersiz kıl
        var otherActiveTokens = await _dbContext.EmailConfirmationTokens
            .Where(t => t.UserId == user.Id && !t.IsUsed && t.Id != tokenRecord.Id)
            .ToListAsync(cancellationToken);

        foreach (var otherToken in otherActiveTokens)
        {
            otherToken.IsUsed = true;
            otherToken.UsedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
