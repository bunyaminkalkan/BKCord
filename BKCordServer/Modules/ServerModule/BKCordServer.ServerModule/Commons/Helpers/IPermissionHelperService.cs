using BKCordServer.ServerModule.Contracts;

namespace BKCordServer.ServerModule.Commons.Helpers;
public interface IPermissionHelperService
{
    Task<bool> IsUserHavePermissionByUserIdAndServerId(Guid userId, Guid serverId, RolePermission permission);
    Task<bool> IsUserHavePermissionByUserIdAndRoleId(Guid userId, Guid roleId, RolePermission permission);
}
