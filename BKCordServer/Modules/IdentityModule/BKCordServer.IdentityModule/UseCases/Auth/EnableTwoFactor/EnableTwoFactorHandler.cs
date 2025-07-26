using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.EnableTwoFactor;
public class EnableTwoFactorHandler : IRequestHandler<EnableTwoFactorCommand, string[]>
{
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly ITwoFactorAuthService _twoFactorAuthService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EnableTwoFactorHandler(UserManager<Domain.Entities.User> userManager, ITwoFactorAuthService twoFactorAuthService, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _twoFactorAuthService = twoFactorAuthService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string[]> Handle(EnableTwoFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
            ?? throw new BadRequestException("User not found");

        var success = await _twoFactorAuthService.EnableTwoFactorAsync(user, request.VerificationCode);

        if (!success)
            throw new BadRequestException("Invalid verification code. Please try again.");

        var recoveryCodes = await _twoFactorAuthService.GenerateRecoveryCodesAsync(user);
        return recoveryCodes;
    }
}
