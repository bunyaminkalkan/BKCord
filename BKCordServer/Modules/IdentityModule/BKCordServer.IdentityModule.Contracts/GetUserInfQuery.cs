using MediatR;

namespace BKCordServer.IdentityModule.Contracts;
public sealed record GetUserInfQuery(Guid UserId) : IRequest<UserInfDTO>;
