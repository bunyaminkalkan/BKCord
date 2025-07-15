using BKCordServer.IdentityModule.Contracts;
using BKCordServer.IdentityModule.Services;
using MediatR;

namespace BKCordServer.IdentityModule.Integrations;
public class GetUserInfHandler : IRequestHandler<GetUserInfQuery, UserInfDTO>
{
    private readonly IUserService _userService;

    public GetUserInfHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserInfDTO> Handle(GetUserInfQuery request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(request.UserId);

        return new UserInfDTO(user.Id, user.UserName, user.AvatarUrl);
    }
}
