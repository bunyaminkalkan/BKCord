using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;

namespace BKCordServer.ServerModule.UseCases.RoleMember.DeleteRoleFromUser;
public sealed class DeleteRoleFromUserHandler : IRequestHandler<DeleteRoleFromUserCommand>
{
    private readonly IRoleMemberService _roleMemberService;

    public DeleteRoleFromUserHandler(IRoleMemberService roleMemberService)
    {
        _roleMemberService = roleMemberService;
    }

    public async Task Handle(DeleteRoleFromUserCommand request, CancellationToken cancellationToken) =>
        await _roleMemberService.DeleteRoleFromUserAsync(request.UserId, request.RoleId);
}
