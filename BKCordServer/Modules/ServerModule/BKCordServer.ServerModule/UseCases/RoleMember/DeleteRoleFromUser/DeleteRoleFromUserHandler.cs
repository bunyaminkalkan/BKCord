using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Contracts;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.RoleMember.DeleteRoleFromUser;
public sealed class DeleteRoleFromUserHandler : IRequestHandler<DeleteRoleFromUserCommand>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IServerAuthorizationService _permissionHelperService;

    public DeleteRoleFromUserHandler(AppServerDbContext dbContext, IHttpContextService httpContextService, IServerAuthorizationService permissionHelperService)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _permissionHelperService = permissionHelperService;
    }

    public async Task Handle(DeleteRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var serverId = (await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId))?.ServerId ??
            throw new NotFoundException($"Server cannot be find with {request.RoleId} role id");

        await _permissionHelperService.ValidateUserHavePermissionByUserIdAndServerId(userId, serverId, RolePermission.ManageRoles);

        var roleMember = await _dbContext.RoleMembers.FirstOrDefaultAsync(rm => rm.UserId == userId && rm.RoleId == request.RoleId)
            ?? throw new NotFoundException($"Role Member cannot be find with {userId} user id and {request.RoleId} role id");

        _dbContext.RoleMembers.Remove(roleMember);
        await _dbContext.SaveChangesAsync();
    }
}
