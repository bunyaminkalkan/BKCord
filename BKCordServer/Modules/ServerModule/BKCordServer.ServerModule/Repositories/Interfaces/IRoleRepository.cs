using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.Repositories.Interfaces;
public interface IRoleRepository
{
    Task AddAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(Role role);
    Task<Role?> GetByIdAsync(Guid id);
    Task<IEnumerable<Role>> GetAllByIdsAsync(IEnumerable<Guid> ids);
    Task<IEnumerable<Role>> GetAllByServerIdAsync(Guid serverId);
    Task<Role?> GetByServerIdAndNameAsync(Guid serverId, string name);
}
