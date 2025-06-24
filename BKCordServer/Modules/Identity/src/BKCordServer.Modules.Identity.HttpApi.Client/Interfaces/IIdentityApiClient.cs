using BKCordServer.Modules.Identity.HttpApi.Client.DTOs;

namespace BKCordServer.Modules.Identity.HttpApi.Client.Interfaces;
public interface IIdentityApiClient
{
    Task RegisterAsync(string email, string userName, string password);
    Task<UserDto> GetByEmailAsync(string email);
}
