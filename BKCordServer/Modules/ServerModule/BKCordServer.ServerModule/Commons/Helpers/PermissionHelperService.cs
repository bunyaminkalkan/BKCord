using BKCordServer.ServerModule.Domain.Enums;
using BKCordServer.ServerModule.Services.Interfaces;

namespace BKCordServer.ServerModule.Commons.Helpers;
public class PermissionHelperService : IPermissionHelperService
{
    private readonly IRoleService _roleService;
    private readonly IRoleMemberService _roleMemberService;
    private readonly IServerService _serverService;

    public PermissionHelperService(IRoleService roleService, IRoleMemberService roleMemberService, IServerService serverService)
    {
        _roleService = roleService;
        _roleMemberService = roleMemberService;
        _serverService = serverService;
    }

    public async Task<bool> IsUserHavePermissionByUserIdAndServerId(Guid userId, Guid serverId, RolePermission permission)
    {
        var roleIds = await _roleMemberService.GetRoleIdsByUserIdAsync(userId);

        var roles = await _roleService.GetAllByIdsAndServerIdAsync(roleIds, serverId);

        var isOwner = await _serverService.IsUserOwnerTheServer(userId, serverId);

        var isHavePermission = roles
            .Any(r => r.RolePermissions.Contains(permission) || r.RolePermissions.Contains(RolePermission.Administrator));

        return isHavePermission || isOwner;
    }

    public async Task<bool> IsUserHavePermissionByUserIdAndRoleId(Guid userId, Guid roleId, RolePermission permission)
    {
        var serverId = await _roleService.GetServerIdByRoleIdAsync(roleId);

        var roleIds = await _roleMemberService.GetRoleIdsByUserIdAsync(userId);

        var roles = await _roleService.GetAllByIdsAndServerIdAsync(roleIds, serverId);

        var isOwner = await _serverService.IsUserOwnerTheServer(userId, serverId);

        var isHavePermission = roles
            .Any(r => r.RolePermissions.Contains(permission) || r.RolePermissions.Contains(RolePermission.Administrator));

        return isHavePermission || isOwner;
    }
}
