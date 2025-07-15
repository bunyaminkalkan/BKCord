using BKCordServer.IdentityModule.Domain.Entities;

namespace BKCordServer.IdentityModule.Services;

public interface IUserService
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByIdAsync(Guid userId);
    Task<IEnumerable<User>> GetAllByIdsAsync(IEnumerable<Guid> userIds);
}
