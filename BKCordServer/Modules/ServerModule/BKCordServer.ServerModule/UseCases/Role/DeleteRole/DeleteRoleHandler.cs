using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;

namespace BKCordServer.ServerModule.UseCases.Role.DeleteRole;
public sealed class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand>
{
    private readonly IRoleService _roleService;
    private readonly IRoleMemberService _roleMemberService;

    public DeleteRoleHandler(IRoleService roleService, IRoleMemberService roleMemberService)
    {
        _roleService = roleService;
        _roleMemberService = roleMemberService;
    }

    public async Task Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        await _roleMemberService.DeleteAllMembersAsync(request.RoleId);
        await _roleService.DeleteAsync(request);
    }
}
