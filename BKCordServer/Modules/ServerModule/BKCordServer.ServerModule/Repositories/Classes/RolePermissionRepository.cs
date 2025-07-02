using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.ServerModule.Repositories.Classes;
public class RolePermissionRepository : IRolePermissionRepository
{
    private readonly AppServerDbContext _dbContext;

    public RolePermissionRepository(AppServerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(RolePermission rolePermission)
    {
        await _dbContext.RolePermissions.AddAsync(rolePermission);
    }

    public async Task UpdateAsync(RolePermission rolePermission)
    {
        _dbContext.RolePermissions.Update(rolePermission);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(RolePermission rolePermission)
    {
        _dbContext.RolePermissions.Remove(rolePermission);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<RolePermission?> GetByRoleIdAsync(Guid roleId) =>
        await _dbContext.RolePermissions.FirstOrDefaultAsync(rp => rp.RoleId == roleId);
}
