using MediatR;

namespace BKCordServer.ServerModule.UseCases.RoleMember.AssignRoleToUser;
public sealed record AssignRoleToUserCommand(Guid UserId, Guid RoleId) : IRequest;
