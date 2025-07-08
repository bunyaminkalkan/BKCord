using BKCordServer.ServerModule.DTOs;
using MediatR;

namespace BKCordServer.ServerModule.UseCases.ServerMember.GetServerUsers;
public sealed record GetServerUsersQuery(Guid ServerId) : IRequest<IEnumerable<RoleBasedServerUsersDTO>>;
