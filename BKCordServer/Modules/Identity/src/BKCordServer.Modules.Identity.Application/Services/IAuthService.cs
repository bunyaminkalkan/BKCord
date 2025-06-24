namespace BKCordServer.Modules.Identity.Application.Services;
public interface IAuthService
{
    Task RegisterAsync(string email, string userName, string password);
}
