using MediatR;

namespace BKCordServer.ServerModule.UseCases.Role.GetServerRoles;
public sealed record GetServerRolesQuery(Guid ServerId) : IRequest<IEnumerable<Domain.Entities.Role>>;
