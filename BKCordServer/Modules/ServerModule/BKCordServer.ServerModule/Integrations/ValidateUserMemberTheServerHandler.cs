using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Contracts;
using MediatR;

namespace BKCordServer.ServerModule.Integrations;
public class ValidateUserMemberTheServerHandler : IRequestHandler<ValidateUserMemberTheServerQuery>
{
    private readonly IServerAuthorizationService _serverAuthorizationService;

    public ValidateUserMemberTheServerHandler(IServerAuthorizationService serverAuthorizationService)
    {
        _serverAuthorizationService = serverAuthorizationService;
    }

    public async Task Handle(ValidateUserMemberTheServerQuery request, CancellationToken cancellationToken) =>
        await _serverAuthorizationService.ValidateUserMemberTheServerByUserIdAndServerId(request.UserId, request.ServerId);
}
