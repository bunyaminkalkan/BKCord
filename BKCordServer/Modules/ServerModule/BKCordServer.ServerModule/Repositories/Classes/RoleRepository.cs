using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.ServerModule.Repositories.Classes;
public class RoleRepository : IRoleRepository
{
    private readonly AppServerDbContext _dbContext;

    public RoleRepository(AppServerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Role role)
    {
        await _dbContext.Roles.AddAsync(role);
        await _dbContext.SaveChangesAsync();
    }
    public async Task UpdateAsync(Role role)
    {
        _dbContext.Roles.Update(role);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Role role)
    {
        _dbContext.Roles.Remove(role);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<Role?> GetByIdAsync(Guid id) =>
        await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Role>> GetAllByIdsAsync(IEnumerable<Guid> ids) =>
        await _dbContext.Roles.Where(r => ids.Contains(r.Id)).ToListAsync();

    public async Task<IEnumerable<Role>> GetAllByServerIdAsync(Guid serverId) =>
        await _dbContext.Roles.Where(r => r.ServerId == serverId).OrderByDescending(r => r.Hierarchy).ToListAsync();

    public async Task<Role?> GetByServerIdAndNameAsync(Guid serverId, string name) =>
        await _dbContext.Roles.FirstOrDefaultAsync(r => r.ServerId == serverId && r.Name == name);

    public async Task<IEnumerable<Role>> GetAllByIdsAndServerIdAsync(IEnumerable<Guid> roleIds, Guid serverId) =>
        await _dbContext.Roles.Where(r => r.ServerId == serverId && roleIds.Contains(r.Id)).ToListAsync();

    public async Task<Guid?> GetServerIdByRoleIdAsync(Guid roleId) =>
        (await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId))?.ServerId;
}
