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
        var server = await _dbContext.Servers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == serverId);

        if (server is null)
            throw new NotFoundException("Server not found");

        if (server.OwnerId == userId)
            return;

        var hasPermission = await _dbContext.RoleMembers
            .Where(rm => rm.UserId == userId)
            .Join(_dbContext.Roles,
                  rm => rm.RoleId,
                  r => r.Id,
                  (rm, r) => new { r.ServerId, r.RolePermissions })
            .Where(x => x.ServerId == serverId)
            .AnyAsync(x =>
                x.RolePermissions.Contains(permission) ||
                x.RolePermissions.Contains(RolePermission.Administrator));

        if (!hasPermission)
            throw new ForbiddenException($"You don't have {permission} permission on this server");
    }



    public async Task ValidateUserMemberTheServerByUserIdAndServerId(Guid userId, Guid serverId)
    {
        var isMember = await _dbContext.ServerMembers.AnyAsync(sm => sm.UserId == userId && sm.ServerId == serverId);
        if (!isMember)
            throw new BadRequestException("The user is not a member of the given server.");
    }
}
