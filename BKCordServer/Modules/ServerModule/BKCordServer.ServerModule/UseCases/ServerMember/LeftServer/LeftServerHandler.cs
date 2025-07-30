using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.ServerMember.LeftServer;
public class LeftServerHandler : IRequestHandler<LeftServerCommand>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IServerAuthorizationService _serverAuthorizationService;

    public LeftServerHandler(AppServerDbContext dbContext, IHttpContextService httpContextService, IServerAuthorizationService serverAuthorizationService)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _serverAuthorizationService = serverAuthorizationService;
    }

    public async Task Handle(LeftServerCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        await _serverAuthorizationService
            .ValidateUserMemberTheServerByUserIdAndServerId(userId, request.ServerId);

        var serverMember = await _dbContext.ServerMembers
            .FirstOrDefaultAsync(sm => sm.UserId == userId && sm.ServerId == request.ServerId, cancellationToken)
            ?? throw new NotFoundException($"Server member cannot be found for user ID {userId} and server ID {request.ServerId}");

        var serverMemberHistory = await _dbContext.ServerMembersHistory
            .FirstOrDefaultAsync(smh => smh.UserId == userId && smh.ServerId == request.ServerId, cancellationToken)
            ?? throw new NotFoundException($"History cannot be found for server ID {request.ServerId} and user ID {userId}");

        serverMemberHistory.UpdatedAt = DateTime.UtcNow;

        _dbContext.ServerMembers.Remove(serverMember);
        _dbContext.ServerMembersHistory.Update(serverMemberHistory);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
