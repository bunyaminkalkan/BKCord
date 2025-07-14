using MediatR;

namespace BKCordServer.ServerModule.Contracts;
public sealed record IsUserMemberTheServerQuery(Guid UserId, Guid ServerId) : IRequest<bool>;
