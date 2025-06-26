using BKCordServer.Modules.Identity.Application.Features.Commands.Register;

namespace BKCordServer.Modules.Identity.Application.Services;
public interface IAuthService
{
    Task RegisterAsync(RegisterCommand request);
}
