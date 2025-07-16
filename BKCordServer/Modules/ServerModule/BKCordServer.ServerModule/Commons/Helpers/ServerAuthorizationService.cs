using BKCordServer.ServerModule.Contracts;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;

namespace BKCordServer.ServerModule.Commons.Helpers;
public class ServerAuthorizationService : IServerAuthorizationService
{
    private readonly AppServerDbContext _dbContext;

    public ServerAuthorizationService(AppServerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ValidateUserHavePermissionByUserIdAndServerId(Guid userId, Guid serverId, RolePermission permission)
    {
        var roleIds = await _dbContext.RoleMembers.Where(rm => rm.UserId == userId).Select(rm => rm.RoleId).ToListAsync();

        var roles = await _dbContext.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();

        var isOwner = (await _dbContext.Servers.FirstOrDefaultAsync(s => s.Id == serverId))?.OwnerId == userId;

        var isHavePermission = roles
            .Any(r => r.RolePermissions.Contains(permission) || r.RolePermissions.Contains(RolePermission.Administrator));

        if (!isHavePermission && !isOwner)
            throw new ForbiddenException($"You don't have {permission.ToString()} permission");
    }

    public async Task ValidateUserMemberTheServerByUserIdAndServerId(Guid userId, Guid serverId)
    {
        var isMember = await _dbContext.ServerMembers.AnyAsync(sm => sm.UserId == userId && sm.ServerId == serverId);
        if (!isMember)
            throw new BadRequestException("The user is not a member of the given server.");
    }
}
