using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.SendTwoFactorVerificationCode;
public class SendTwoFactorVerificationCodeHandler : IRequestHandler<SendTwoFactorVerificationCodeCommand>
{
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly ITwoFactorAuthService _twoFactorAuthService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SendTwoFactorVerificationCodeHandler(UserManager<Domain.Entities.User> userManager, ITwoFactorAuthService twoFactorAuthService, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _twoFactorAuthService = twoFactorAuthService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(SendTwoFactorVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
            ?? throw new BadRequestException("User not found");

        var success = await _twoFactorAuthService.SendTwoFactorCodeAsync(user);

        if (!success)
            throw new BadRequestException("Failed to send verification code. Please try again.");
    }
}
