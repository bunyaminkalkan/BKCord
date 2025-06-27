using BKCordServer.Identity.UseCases.Auth.Register;

namespace BKCordServer.Identity.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterCommand request);
}
