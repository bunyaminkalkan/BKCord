using MediatR;

namespace BKCordServer.ServerModule.UseCases.Role.CreateRole;
public sealed record CreateRoleCommand(
    Guid ServerId,
    string Name,
    string Color,
    short Hierarchy
    ) : IRequest;
