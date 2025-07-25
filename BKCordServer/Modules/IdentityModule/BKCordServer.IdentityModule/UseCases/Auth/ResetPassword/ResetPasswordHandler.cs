using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.ResetPassword;
public sealed class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly IPasswordResetTokenService _tokenService;

    public ResetPasswordHandler(
        AppIdentityDbContext dbContext,
        UserManager<Domain.Entities.User> userManager,
        IPasswordResetTokenService tokenService)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var tokenRecord = await _dbContext.ForgotPasswordTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken)
            ?? throw new NotFoundException("Invalid reset link.");

        if (tokenRecord.IsUsed)
            throw new BadRequestException("This reset link has already been used.");

        if (tokenRecord.ExpiresAt < DateTime.UtcNow)
            throw new BadRequestException("The reset link has expired.");

        // Token doğrulama
        if (!_tokenService.ValidateToken(request.Token, tokenRecord.Token))
            throw new BadRequestException("Invalid reset link.");

        var user = tokenRecord.User;

        // Aynı şifre kontrolü
        var isSamePassword = await _userManager.CheckPasswordAsync(user, request.NewPassword);
        if (isSamePassword)
            throw new BadRequestException("The new password cannot be the same as the old one.");

        // Şifreyi güncelle
        await _userManager.RemovePasswordAsync(user);
        var result = await _userManager.AddPasswordAsync(user, request.NewPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"An error occurred while updating the password: {errors}");
        }

        // Token'ı kullanılmış olarak işaretle
        tokenRecord.IsUsed = true;
        tokenRecord.UsedAt = DateTime.UtcNow;

        // Kullanıcının diğer aktif tokenlarını da geçersiz kıl
        var otherActiveTokens = await _dbContext.ForgotPasswordTokens
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
