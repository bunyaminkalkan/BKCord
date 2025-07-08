using BKCordServer.IdentityModule.Contracts;
using BKCordServer.IdentityModule.Services;
using MediatR;

namespace BKCordServer.IdentityModule.Integrations;
public sealed class ListUserInfsHandler : IRequestHandler<ListUserInfsQuery, IEnumerable<UserInfDTO>>
{
    private readonly IUserService _userService;

    public ListUserInfsHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IEnumerable<UserInfDTO>> Handle(ListUserInfsQuery request, CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllByIdsAsync(request.UserIds);

        return users.Select(u => new UserInfDTO(u.Id, u.UserName, u.AvatarUrl));
    }
}
