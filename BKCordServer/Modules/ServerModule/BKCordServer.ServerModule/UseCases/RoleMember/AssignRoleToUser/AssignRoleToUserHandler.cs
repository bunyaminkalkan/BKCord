using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;

namespace BKCordServer.ServerModule.UseCases.RoleMember.AssignRoleToUser;
public sealed class AssignRoleToUserHandler : IRequestHandler<AssignRoleToUserCommand>
{
    private readonly IRoleMemberService _roleMemberService;

    public AssignRoleToUserHandler(IRoleMemberService roleMemberService)
    {
        _roleMemberService = roleMemberService;
    }

    public async Task Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken) =>
        await _roleMemberService.AssignRoleToUserAsync(request.UserId, request.RoleId);
}
