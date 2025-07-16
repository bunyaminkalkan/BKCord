using MediatR;

namespace BKCordServer.ServerModule.Contracts;
public sealed record ValidateUserMemberTheServerQuery(Guid UserId, Guid ServerId) : IRequest;
