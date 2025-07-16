using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Server.DeleteServer;
public sealed class DeleteServerHandler : IRequestHandler<DeleteServerCommand>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;

    public DeleteServerHandler(AppServerDbContext dbContext, IHttpContextService httpContextService)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
    }

    public async Task Handle(DeleteServerCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var server = await _dbContext.Servers.FirstOrDefaultAsync(s => s.Id == request.ServerId)
            ?? throw new NotFoundException($"Server cannot be find with {request.ServerId} server id");

        if (server.OwnerId != userId)
            throw new ForbiddenException("You don't have permission to delete this server");

        _dbContext.Servers.Remove(server);
        await _dbContext.SaveChangesAsync();
    }
}
