using BKCordServer.IdentityModule.Domain.Entities;

namespace BKCordServer.IdentityModule.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, string password);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllByIdsAsync(IEnumerable<Guid> userIds);
}
