using MediatR;

namespace BKCordServer.IdentityModule.Contracts;
public sealed record ListUserInfsQuery(IEnumerable<Guid> UserIds) : IRequest<IEnumerable<UserInfDTO>>;
