using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.Repositories.Interfaces;
public interface IRolePermissionRepository
{
    Task AddAsync(RolePermission rolePermission);
    Task UpdateAsync(RolePermission rolePermission);
    Task DeleteAsync(RolePermission rolePermission);
    Task<RolePermission?> GetByRoleIdAsync(Guid roleId);
}
