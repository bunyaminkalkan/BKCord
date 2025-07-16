using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Contracts;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Role.DeleteRole;
public sealed class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IServerAuthorizationService _permissionHelperService;

    public DeleteRoleHandler(
        AppServerDbContext dbContext,
        IHttpContextService httpContextService,
        IServerAuthorizationService permissionHelperService)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _permissionHelperService = permissionHelperService;
    }

    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId)
            ?? throw new NotFoundException($"Role cannot be find with {request.RoleId} role id");

        await _permissionHelperService.ValidateUserHavePermissionByUserIdAndServerId(userId, role.ServerId, RolePermission.ManageRoles);

        var roleMembers = await _dbContext.RoleMembers.Where(rm => rm.RoleId == request.RoleId).ToListAsync();

        _dbContext.Roles.Remove(role);
        _dbContext.RoleMembers.RemoveRange(roleMembers);

        await _dbContext.SaveChangesAsync();
    }
}
