using MediatR;

namespace BKCordServer.ServerModule.UseCases.Server.DeleteServer;
public sealed record DeleteServerCommand(Guid ServerId) : IRequest;
