using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.UseCases.Role.CreateRole;

namespace BKCordServer.ServerModule.Services.Interfaces;
public interface IRoleService
{
    Task CreateAsync(CreateRoleCommand request);
    Task DeleteAsync(Role role);
    Task<Role> GetByIdAsync(Guid id);
    Task<IEnumerable<Role>> GetAllByIdsAsync(IEnumerable<Guid> ids);
    Task<IEnumerable<Role>> GetAllByServerIdAsync(Guid serverId);
    Task ValidateRoleExist(Guid serverId, string name);
}
