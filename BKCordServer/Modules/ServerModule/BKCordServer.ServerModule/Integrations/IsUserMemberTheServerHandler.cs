using BKCordServer.ServerModule.Contracts;
using BKCordServer.ServerModule.Services.Interfaces;
using MediatR;

namespace BKCordServer.ServerModule.Integrations;
public class IsUserMemberTheServerHandler : IRequestHandler<IsUserMemberTheServerQuery, bool>
{
    private readonly IServerMemberService _serverMemberService;

    public IsUserMemberTheServerHandler(IServerMemberService serverMemberService)
    {
        _serverMemberService = serverMemberService;
    }

    public async Task<bool> Handle(IsUserMemberTheServerQuery request, CancellationToken cancellationToken) =>
        await _serverMemberService.IsUserMemberTheServer(request.UserId, request.ServerId);
}
