using BKCordServer.IdentityModule.DTOs;
using BKCordServer.IdentityModule.Services;
using MediatR;

namespace BKCordServer.IdentityModule.UseCases.User.GetByEmail;
public sealed class GetByEmailHandler : IRequestHandler<GetByEmailQuery, UserDTO>
{
    private readonly IUserService _userService;

    public GetByEmailHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserDTO> Handle(GetByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByEmailAsync(request.Email);

        return new UserDTO(user);
    }
}
