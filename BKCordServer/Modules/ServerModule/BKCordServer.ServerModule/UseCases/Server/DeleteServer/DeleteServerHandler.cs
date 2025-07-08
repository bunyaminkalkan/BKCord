using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Server.DeleteServer;
public sealed class DeleteServerHandler : IRequestHandler<DeleteServerCommand>
{
    private readonly IServerService _serverService;
    private readonly IHttpContextService _httpContextService;

    public DeleteServerHandler(IServerService serverService, IHttpContextService httpContextService)
    {
        _serverService = serverService;
        _httpContextService = httpContextService;
    }

    public async Task Handle(DeleteServerCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var server = await _serverService.GetByIdAsync(request.ServerId)
            ?? throw new NotFoundException($"Server cannot be find with {request.ServerId} server id");

        if (server.OwnerId != userId)
            throw new ForbiddenException("You don't have permission to delete this server");

        await _serverService.DeleteAsync(server);
    }
}
