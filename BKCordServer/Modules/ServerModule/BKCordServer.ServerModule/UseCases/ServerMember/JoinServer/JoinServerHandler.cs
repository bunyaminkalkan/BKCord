using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using BKCordServer.ServerModule.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.ServerMember.JoinServer;
public class JoinServerHandler : IRequestHandler<JoinServerCommand>
{
    private readonly AppServerDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;

    public JoinServerHandler(AppServerDbContext dbContext, IHttpContextService httpContextService)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
    }

    public async Task Handle(JoinServerCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var server = await _dbContext.Servers
            .AsNoTracking()
            .SingleOrDefaultAsync(s => s.InviteCode == request.InviteCode, cancellationToken);

        if (server is null)
            throw new NotFoundException($"Server not found with invite code: {request.InviteCode}");

        var serverId = server.Id;

        var isMember = await _dbContext.ServerMembers
            .AnyAsync(sm => sm.UserId == userId && sm.ServerId == serverId, cancellationToken);

        if (isMember)
            throw new BadRequestException("User is already a member of the server.");

        var isBanned = await _dbContext.ServerMembersHistory
            .AnyAsync(smh => smh.UserId == userId && smh.ServerId == serverId && smh.IsBanned, cancellationToken);

        if (isBanned)
            throw new BadRequestException("User has been banned from this server.");

        var serverMember = new Domain.Entities.ServerMember
        {
            UserId = userId,
            ServerId = serverId
        };

        var serverMemberHistory = new ServerMemberHistory
        {
            UserId = userId,
            ServerId = serverId,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.ServerMembers.Add(serverMember);
        _dbContext.ServerMembersHistory.Add(serverMemberHistory);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
