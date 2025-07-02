using MediatR;

namespace BKCordServer.ServerModule.UseCases.ServerMember.JoinServer;
public sealed record JoinServerCommand(string InviteCode) : IRequest;
