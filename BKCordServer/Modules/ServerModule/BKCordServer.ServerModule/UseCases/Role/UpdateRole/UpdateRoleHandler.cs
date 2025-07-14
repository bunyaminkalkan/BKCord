using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Contracts;
using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Role.UpdateRole;
public sealed class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, Domain.Entities.Role>
{
    private readonly IRoleService _roleService;
    private readonly IHttpContextService _httpContextService;
    private readonly IPermissionHelperService _permissionHelperService;

    public UpdateRoleHandler(IRoleService roleService, IHttpContextService httpContextService, IPermissionHelperService permissionHelperService)
    {
        _roleService = roleService;
        _httpContextService = httpContextService;
        _permissionHelperService = permissionHelperService;
    }

    public async Task<Domain.Entities.Role> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var isHavePermission = await _permissionHelperService.IsUserHavePermissionByUserIdAndRoleId(userId, request.RoleId, RolePermission.ManageRoles);

        if (!isHavePermission)
            throw new ForbiddenException("You don't have manage roles permission");

        var role = await _roleService.UpdateAsync(request);

        return role;
    }
}
