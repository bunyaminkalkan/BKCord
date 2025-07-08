using BKCordServer.IdentityModule.Domain.Entities;

namespace BKCordServer.IdentityModule.Services;

public interface IUserService
{
    Task<User> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllByIdsAsync(IEnumerable<Guid> userIds);
}
