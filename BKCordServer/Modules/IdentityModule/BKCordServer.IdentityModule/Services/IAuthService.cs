using BKCordServer.IdentityModule.DTOs;
using BKCordServer.IdentityModule.UseCases.Auth.Login;
using BKCordServer.IdentityModule.UseCases.Auth.Register;

namespace BKCordServer.IdentityModule.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterCommand request);
    Task<JwtResponse> LoginAsync(LoginCommand request);
}
