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

    public async Task<IEnumerable<Guid>> GetRoleIdsByUserIdAndServerIdAsync(Guid userId, Guid serverId) =>
        await _dbContext.RoleMembers.Where(rm => rm.UserId == userId && rm.ServerId == serverId).Select(rm => rm.RoleId).ToListAsync();
}
