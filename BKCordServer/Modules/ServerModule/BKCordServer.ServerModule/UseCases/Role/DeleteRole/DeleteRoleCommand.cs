using MediatR;

namespace BKCordServer.ServerModule.UseCases.Role.DeleteRole;
public sealed record DeleteRoleCommand(Guid RoleId) : IRequest;
