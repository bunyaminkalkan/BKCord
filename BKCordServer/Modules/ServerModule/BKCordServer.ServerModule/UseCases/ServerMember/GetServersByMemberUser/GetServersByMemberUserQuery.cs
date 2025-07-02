using BKCordServer.ServerModule.DTOs;
using MediatR;

namespace BKCordServer.ServerModule.UseCases.ServerMember.GetServersByMemberUser;
public record GetServersByMemberUserQuery() : IRequest<IEnumerable<ServerDTO>>;
