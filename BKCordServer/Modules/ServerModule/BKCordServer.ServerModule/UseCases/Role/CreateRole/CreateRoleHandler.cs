using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Domain.Enums;
using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Role.CreateRole;
public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, Domain.Entities.Role>
{
    private readonly IRoleService _roleService;
    private readonly IHttpContextService _httpContextService;
    private readonly IPermissionHelperService _permissionHelperService;

    public CreateRoleHandler(IRoleService roleService, IHttpContextService httpContextService, IPermissionHelperService permissionHelperService)
    {
        _roleService = roleService;
        _httpContextService = httpContextService;
        _permissionHelperService = permissionHelperService;
    }

    public async Task<Domain.Entities.Role> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var isHavePermission = await _permissionHelperService.IsUserHavePermissionByUserIdAndServerId(userId, request.ServerId, RolePermission.ManageRoles);

        if (!isHavePermission)
            throw new ForbiddenException("You don't have manage roles permission");

        await _roleService.ValidateRoleExist(request.ServerId, request.Name);

        var role = await _roleService.CreateAsync(request);

        return role;
    }
}
