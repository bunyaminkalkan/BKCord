using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Contracts;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Role.UpdateRole;
public sealed class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, Domain.Entities.Role>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IServerAuthorizationService _permissionHelperService;

    public UpdateRoleHandler(AppServerDbContext dbContext, IHttpContextService httpContextService, IServerAuthorizationService permissionHelperService)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _permissionHelperService = permissionHelperService;
    }

    public async Task<Domain.Entities.Role> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId)
            ?? throw new NotFoundException($"Role cannot be find with {request.RoleId} role id");

        await _permissionHelperService.ValidateUserHavePermissionByUserIdAndServerId(userId, role.ServerId, RolePermission.ManageRoles);

        role.Name = request.Name;
        role.Color = request.Color;
        role.Hierarchy = request.Hierarchy;
        role.RolePermissions = request.RolePermissions;

        _dbContext.Roles.Update(role);
        await _dbContext.SaveChangesAsync();

        return role;
    }
}
