using BKCordServer.ServerModule.DTOs;
using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.ServerMember.GetServersByMemberUser;
public class GetServersByMemberUserHandler : IRequestHandler<GetServersByMemberUserQuery, IEnumerable<ServerDTO>>
{
    private readonly IServerService _serverService;
    private readonly IServerMemberService _serverMemberService;
    private readonly IHttpContextService _httpContextService;

    public GetServersByMemberUserHandler(IServerService serverService, IServerMemberService serverMemberService, IHttpContextService httpContextService)
    {
        _serverService = serverService;
        _serverMemberService = serverMemberService;
        _httpContextService = httpContextService;
    }

    public async Task<IEnumerable<ServerDTO>> Handle(GetServersByMemberUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        var serverIds = await _serverMemberService.GetServerIdsByUserIdAsync(userId);

        var servers = await _serverService.GetAllByIdsAsync(serverIds);

        return servers.Select(server => new ServerDTO(server));
    }
}
