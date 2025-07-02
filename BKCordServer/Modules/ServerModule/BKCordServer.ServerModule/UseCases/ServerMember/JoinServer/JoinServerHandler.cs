using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.ServerMember.JoinServer;
public class JoinServerHandler : IRequestHandler<JoinServerCommand>
{
    private readonly IServerMemberService _serverMemberService;
    private readonly IServerMembersHistoryService _serverMembersHistoryService;
    private readonly IServerService _serverService;
    private readonly IHttpContextService _httpContextService;

    public JoinServerHandler(
        IServerMemberService serverMemberService,
        IServerMembersHistoryService serverMembersHistoryService,
        IServerService serverService,
        IHttpContextService httpContextService
        )
    {
        _serverMemberService = serverMemberService;
        _serverMembersHistoryService = serverMembersHistoryService;
        _serverService = serverService;
        _httpContextService = httpContextService;
    }

    public async Task Handle(JoinServerCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();
        var serverId = await _serverService.GetServerIdByInviteCodeAsync(request.InviteCode);

        await _serverMemberService.JoinServerAsync(userId, serverId);
        await _serverMembersHistoryService.JoinServerAsync(userId, serverId);
    }
}
