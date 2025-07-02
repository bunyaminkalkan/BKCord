using MediatR;

namespace BKCordServer.ServerModule.UseCases.ServerMember.LeftServer;
public sealed record LeftServerCommand(Guid ServerId) : IRequest;
