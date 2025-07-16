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

        await _serverAuthorizationService.ValidateUserMemberTheServerByUserIdAndServerId(userId, request.ServerId);

        var serverMember = await _dbContext.ServerMembers.FirstOrDefaultAsync(sm => sm.UserId == userId && sm.ServerId == request.ServerId)
            ?? throw new NotFoundException($"Server member connot be find {userId} user id and {request.ServerId} server id");

        var serverMembersHistory = await _dbContext.ServerMembersHistory.FirstOrDefaultAsync(smh => smh.UserId == userId && smh.ServerId == request.ServerId)
            ?? throw new NotFoundException($"History connot be find with {request.ServerId} server id and {userId} user id");

        _dbContext.ServerMembers.Remove(serverMember);
        _dbContext.ServerMembersHistory.Remove(serverMembersHistory);

        await _dbContext.SaveChangesAsync();
    }
}
