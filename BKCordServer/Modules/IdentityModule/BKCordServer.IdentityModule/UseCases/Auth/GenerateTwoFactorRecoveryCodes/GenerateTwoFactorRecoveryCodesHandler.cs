using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.GenerateTwoFactorRecoveryCodes;
public class GenerateTwoFactorRecoveryCodesHandler : IRequestHandler<GenerateTwoFactorRecoveryCodesCommand, string[]>
{
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly ITwoFactorAuthService _twoFactorAuthService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GenerateTwoFactorRecoveryCodesHandler(UserManager<Domain.Entities.User> userManager, ITwoFactorAuthService twoFactorAuthService, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _twoFactorAuthService = twoFactorAuthService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string[]> Handle(GenerateTwoFactorRecoveryCodesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
            ?? throw new BadRequestException("User not found");

        if (!user.TwoFactorEnabled)
            throw new BadRequestException("Two-factor authentication is not enabled.");

        var recoveryCodes = await _twoFactorAuthService.GenerateRecoveryCodesAsync(user);
        return recoveryCodes;
    }
}
