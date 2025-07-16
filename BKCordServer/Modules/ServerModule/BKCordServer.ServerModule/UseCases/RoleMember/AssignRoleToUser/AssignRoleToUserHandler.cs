using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Contracts;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.RoleMember.AssignRoleToUser;
public sealed class AssignRoleToUserHandler : IRequestHandler<AssignRoleToUserCommand>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IServerAuthorizationService _permissionHelperService;

    public AssignRoleToUserHandler(AppServerDbContext dbContext, IHttpContextService httpContextService, IServerAuthorizationService permissionHelperService)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _permissionHelperService = permissionHelperService;
    }

    public async Task Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var serverId = (await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId))?.ServerId ??
            throw new NotFoundException($"Server cannot be find with {request.RoleId} role id");

        await _permissionHelperService.ValidateUserHavePermissionByUserIdAndServerId(userId, serverId, RolePermission.ManageRoles);

        var roleMember = new Domain.Entities.RoleMember
        {
            UserId = request.UserId,
            RoleId = request.RoleId
        };

        _dbContext.RoleMembers.Add(roleMember);
        await _dbContext.SaveChangesAsync();
    }
}
