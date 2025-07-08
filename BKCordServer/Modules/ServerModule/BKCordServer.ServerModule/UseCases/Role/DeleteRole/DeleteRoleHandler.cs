using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Domain.Enums;
using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Role.DeleteRole;
public sealed class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand>
{
    private readonly IRoleService _roleService;
    private readonly IRoleMemberService _roleMemberService;
    private readonly IHttpContextService _httpContextService;
    private readonly IPermissionHelperService _permissionHelperService;

    public DeleteRoleHandler(
        IRoleService roleService,
        IRoleMemberService roleMemberService,
        IHttpContextService httpContextService,
        IPermissionHelperService permissionHelperService)
    {
        _roleService = roleService;
        _roleMemberService = roleMemberService;
        _httpContextService = httpContextService;
        _permissionHelperService = permissionHelperService;
    }

    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var isHavePermission = await _permissionHelperService.IsUserHavePermissionByUserIdAndRoleId(userId, request.RoleId, RolePermission.ManageRoles);

        if (!isHavePermission)
            throw new ForbiddenException("You don't have manage roles permission");

        await _roleMemberService.DeleteAllMembersAsync(request.RoleId);
        await _roleService.DeleteAsync(request);
    }
}
