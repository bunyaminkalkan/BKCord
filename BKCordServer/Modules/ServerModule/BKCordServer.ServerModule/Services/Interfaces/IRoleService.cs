using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.UseCases.Role.CreateRole;
using BKCordServer.ServerModule.UseCases.Role.DeleteRole;
using BKCordServer.ServerModule.UseCases.Role.UpdateRole;

namespace BKCordServer.ServerModule.Services.Interfaces;
public interface IRoleService
{
    Task CreateAsync(CreateRoleCommand request);
    Task UpdateAsync(UpdateRoleCommand request);
    Task DeleteAsync(DeleteRoleCommand request);
    Task<Role> GetByIdAsync(Guid id);
    Task<IEnumerable<Role>> GetAllByIdsAsync(IEnumerable<Guid> ids);
    Task<IEnumerable<Role>> GetAllByServerIdAsync(Guid serverId);
    Task ValidateRoleExist(Guid serverId, string name);
}
