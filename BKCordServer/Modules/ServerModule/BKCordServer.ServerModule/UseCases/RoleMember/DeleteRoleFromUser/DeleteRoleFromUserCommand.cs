using MediatR;

namespace BKCordServer.ServerModule.UseCases.RoleMember.DeleteRoleFromUser;
public sealed record DeleteRoleFromUserCommand(Guid UserId, Guid RoleId) : IRequest;
