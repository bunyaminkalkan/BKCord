using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Contracts;
using MediatR;

namespace BKCordServer.ServerModule.Integrations;
public class ValidateUserHavePermissionHandler : IRequestHandler<ValidateUserHavePermissionQuery>
{
    private readonly IServerAuthorizationService _serverAuthorizationService;

    public ValidateUserHavePermissionHandler(IServerAuthorizationService serverAuthorizationService)
    {
        _serverAuthorizationService = serverAuthorizationService;
    }

    public async Task Handle(ValidateUserHavePermissionQuery request, CancellationToken cancellationToken) =>
        await _serverAuthorizationService.ValidateUserHavePermissionByUserIdAndServerId(request.UserId, request.ServerId, request.Permission);
}
