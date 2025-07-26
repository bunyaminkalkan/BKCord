using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.GetTwoFactorStatus;
public class GetTwoFactorStatusHandler : IRequestHandler<GetTwoFactorStatusQuery, bool>
{
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetTwoFactorStatusHandler(UserManager<Domain.Entities.User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(GetTwoFactorStatusQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User)
            ?? throw new BadRequestException("User not found");

        return user.TwoFactorEnabled;
    }
}
