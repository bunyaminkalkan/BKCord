using BKCordServer.Modules.Identity.Domain.Entities;

namespace BKCordServer.Modules.Identity.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, string password);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByEmailAsync(string email);
}
