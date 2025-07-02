using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.Repositories.Interfaces;
public interface IRoleRepository
{
    Task AddAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(Role role);
    Task<Role?> GetByIdAsync(Guid id);
    Task<IEnumerable<Role>> GetAllByServerIdAsync(Guid serverId);
}
