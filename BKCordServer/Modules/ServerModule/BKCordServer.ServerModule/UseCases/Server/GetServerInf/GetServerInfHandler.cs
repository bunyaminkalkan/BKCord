using BKCordServer.ServerModule.DTOs;
using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;
using Shared.Kernel.Services;

namespace BKCordServer.ServerModule.UseCases.Server.GetServerInf;
public class GetServerInfHandler : IRequestHandler<GetServerInfQuery, ServerInfDTO>
{
    private readonly IServerService _serverService;
    private readonly IServerMemberService _serverMemberService;
    private readonly IHttpContextService _httpContextService;

    public GetServerInfHandler(IServerService serverService, IServerMemberService serverMemberService, IHttpContextService httpContextService)
    {
        _serverService = serverService;
        _serverMemberService = serverMemberService;
        _httpContextService = httpContextService;
    }

    public async Task<ServerInfDTO> Handle(GetServerInfQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextService.GetUserId();

        await _serverMemberService.ValidateMemberJoinedServer(userId, request.ServerId);

        var server = await _serverService.GetByIdAsync(request.ServerId);

        return new ServerInfDTO(server);
    }
}
