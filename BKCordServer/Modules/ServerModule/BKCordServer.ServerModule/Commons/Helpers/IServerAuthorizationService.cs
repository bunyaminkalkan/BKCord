using BKCordServer.ServerModule.Contracts;

namespace BKCordServer.ServerModule.Commons.Helpers;
public interface IServerAuthorizationService
{
    Task ValidateUserHavePermissionByUserIdAndServerId(Guid userId, Guid serverId, RolePermission permission);
    Task ValidateUserMemberTheServerByUserIdAndServerId(Guid userId, Guid serverId);
}
