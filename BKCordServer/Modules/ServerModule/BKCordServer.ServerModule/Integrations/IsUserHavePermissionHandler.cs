using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Contracts;
using MediatR;

namespace BKCordServer.ServerModule.Integrations;
public class IsUserHavePermissionHandler : IRequestHandler<IsUserHavePermissionQuery, bool>
{
    private readonly IPermissionHelperService _permissionHelperService;

    public IsUserHavePermissionHandler(IPermissionHelperService permissionHelperService)
    {
        _permissionHelperService = permissionHelperService;
    }

    public async Task<bool> Handle(IsUserHavePermissionQuery request, CancellationToken cancellationToken) =>
        await _permissionHelperService.IsUserHavePermissionByUserIdAndServerId(request.UserId, request.ServerId, request.Permission);
}
