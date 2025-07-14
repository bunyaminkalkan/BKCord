using BKCordServer.ServerModule.Contracts;
using MediatR;

namespace BKCordServer.ServerModule.UseCases.Role.CreateRole;
public sealed record CreateRoleCommand(
    Guid ServerId,
    string Name,
    string Color,
    short Hierarchy,
    List<RolePermission> RolePermissions
    ) : IRequest<Domain.Entities.Role>;
