using BKCordServer.Modules.Identity.Application.DTOs;
using BKCordServer.Modules.Identity.Application.Services;
using MediatR;

namespace BKCordServer.Modules.Identity.Application.Features.Quries.GetByEmail;
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
