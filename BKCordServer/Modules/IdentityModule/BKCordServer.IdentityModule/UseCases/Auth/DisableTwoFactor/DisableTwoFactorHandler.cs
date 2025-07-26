using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.DisableTwoFactor;
public class DisableTwoFactorHandler : IRequestHandler<DisableTwoFactorCommand>
{
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly ITwoFactorAuthService _twoFactorAuthService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DisableTwoFactorHandler(UserManager<Domain.Entities.User> userManager, ITwoFactorAuthService twoFactorAuthService, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _twoFactorAuthService = twoFactorAuthService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(DisableTwoFactorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
            ?? throw new BadRequestException("User not found");

        var success = await _twoFactorAuthService.DisableTwoFactorAsync(user);

        if (!success)
            throw new BadRequestException("Operation failed. Please try again.");
    }
}
