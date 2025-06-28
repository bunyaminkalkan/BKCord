using BKCordServer.Identity.DTOs;
using BKCordServer.Identity.Services;
using MediatR;

namespace BKCordServer.Identity.UseCases.Auth.Login;

public class LoginHandler : IRequestHandler<LoginCommand, JwtResponse>
{
    private readonly IAuthService _authService;

    public LoginHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<JwtResponse> Handle(LoginCommand request, CancellationToken cancellationToken) =>
        await _authService.LoginAsync(request);
}
