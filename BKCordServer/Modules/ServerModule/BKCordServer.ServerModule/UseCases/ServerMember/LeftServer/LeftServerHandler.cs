using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.ServerMember.LeftServer;
public class LeftServerHandler : IRequestHandler<LeftServerCommand>
{
    private readonly IServerMemberService _serverMemberService;
    private readonly IServerMembersHistoryService _serverMembersHistoryService;
    private readonly IHttpContextService _httpContextService;

    public LeftServerHandler(
        IServerMemberService serverMemberService,
        IServerMembersHistoryService serverMembersHistoryService,
        IHttpContextService httpContextService
        )
    {
        _serverMemberService = serverMemberService;
        _serverMembersHistoryService = serverMembersHistoryService;
        _httpContextService = httpContextService;
    }

    public async Task Handle(LeftServerCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        await _serverMemberService.LeftServerAsync(userId, request.ServerId);
        await _serverMembersHistoryService.LeftServerAsync(userId, request.ServerId, false, null, null);
    }
}
