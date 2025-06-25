using BKCordServer.Modules.Identity.Domain.Entities;

namespace BKCordServer.Modules.Identity.Application.Services;
public interface IUserService
{
    Task<User> GetByEmailAsync(string email);
}
