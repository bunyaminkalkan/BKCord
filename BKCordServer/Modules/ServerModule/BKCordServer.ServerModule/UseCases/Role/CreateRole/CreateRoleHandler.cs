using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Contracts;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Role.CreateRole;
public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, Domain.Entities.Role>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IServerAuthorizationService _permissionHelperService;

    public CreateRoleHandler(AppServerDbContext dbContext, IHttpContextService httpContextService, IServerAuthorizationService permissionHelperService)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _permissionHelperService = permissionHelperService;
    }

    public async Task<Domain.Entities.Role> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        await _permissionHelperService.ValidateUserHavePermissionByUserIdAndServerId(userId, request.ServerId, RolePermission.ManageRoles);

        var isRoleExist = await _dbContext.Roles.AnyAsync(r => r.ServerId == request.ServerId && r.Name == request.Name);

        if (isRoleExist)
            throw new BadRequestException($"Given '{request.Name}' role already exist");

        var role = new Domain.Entities.Role
        {
            ServerId = request.ServerId,
            Name = request.Name,
            Color = request.Color,
            Hierarchy = request.Hierarchy,
            RolePermissions = request.RolePermissions
        };

        _dbContext.Roles.Add(role);
        await _dbContext.SaveChangesAsync();

        return role;
    }
}
