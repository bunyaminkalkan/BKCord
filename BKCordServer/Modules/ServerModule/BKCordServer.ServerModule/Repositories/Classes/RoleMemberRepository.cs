using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.ServerModule.Repositories.Classes;
public class RoleMemberRepository : IRoleMemberRepository
{
    private readonly AppServerDbContext _dbContext;

    public RoleMemberRepository(AppServerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(RoleMember roleMember)
    {
        await _dbContext.RoleMembers.AddAsync(roleMember);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(RoleMember roleMember)
    {
        _dbContext.RoleMembers.Remove(roleMember);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAllMembersAsync(Guid roleId)
    {
        var roleMembers = await _dbContext.RoleMembers.Where(rm => rm.RoleId == roleId).ToListAsync();

        foreach (var member in roleMembers)
        {
            _dbContext.RoleMembers.Remove(member);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<RoleMember?> GetByUserIdAndRoleIdAsync(Guid userId, Guid roleId) =>
        await _dbContext.RoleMembers.FirstOrDefaultAsync(rm => rm.UserId == userId && rm.RoleId == roleId);

    public async Task<IEnumerable<Guid>> GetRoleIdsByUserIdAsync(Guid userId) =>
        await _dbContext.RoleMembers.Where(rm => rm.UserId == userId).Select(rm => rm.RoleId).ToListAsync();

    public async Task<IEnumerable<Guid>> GetUserIdsByRoleIdAsync(Guid roleId) =>
        await _dbContext.RoleMembers.Where(rm => rm.RoleId == roleId).Select(rm => rm.UserId).ToListAsync();
}
