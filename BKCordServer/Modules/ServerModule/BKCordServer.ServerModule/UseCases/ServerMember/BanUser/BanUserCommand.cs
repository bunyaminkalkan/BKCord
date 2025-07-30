using MediatR;

namespace BKCordServer.ServerModule.UseCases.ServerMember.BanUser;
public sealed record BanUserCommand(Guid UserId, Guid ServerId, string? Reason) : IRequest;
