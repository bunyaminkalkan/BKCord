using BKCordServer.Identity.DTOs;
using BKCordServer.Identity.UseCases.Auth.Login;
using BKCordServer.Identity.UseCases.Auth.Register;

namespace BKCordServer.Identity.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterCommand request);
    Task<JwtResponse> LoginAsync(LoginCommand request);
}
