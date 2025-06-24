using BKCordServer.Modules.Identity.Application.DTOs;

namespace BKCordServer.Modules.Identity.Application.Services;
public interface IUserService
{
    Task<UserDto> GetByEmailAsync(string email);
}
