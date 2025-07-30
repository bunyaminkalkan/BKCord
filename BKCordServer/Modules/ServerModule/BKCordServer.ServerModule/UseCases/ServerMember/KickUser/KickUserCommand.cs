using MediatR;

namespace BKCordServer.ServerModule.UseCases.ServerMember.KickUser;
public sealed record KickUserCommand(Guid UserId, Guid ServerId, string? Reason) : IRequest;
