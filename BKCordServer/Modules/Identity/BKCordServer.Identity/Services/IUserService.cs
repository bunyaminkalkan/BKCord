using BKCordServer.Identity.Domain.Entities;

namespace BKCordServer.Identity.Services;

public interface IUserService
{
    Task<User> GetByEmailAsync(string email);
}
