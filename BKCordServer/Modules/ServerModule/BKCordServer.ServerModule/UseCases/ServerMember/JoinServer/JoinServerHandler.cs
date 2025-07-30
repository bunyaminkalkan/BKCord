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

        var serverId = (await _dbContext.Servers.FirstOrDefaultAsync(s => s.InviteCode == request.InviteCode))?.Id
            ?? throw new NotFoundException($"Server cannot be find with {request.InviteCode} invite code");

        var serverMember = new Domain.Entities.ServerMember
        {
            UserId = userId,
            ServerId = serverId
        };

        var serverMembersHistory = new ServerMemberHistory
        {
            UserId = userId,
            ServerId = serverId
        };

        _dbContext.ServerMembers.Add(serverMember);
        _dbContext.ServerMembersHistory.Add(serverMembersHistory);

        await _dbContext.SaveChangesAsync();
    }
}
