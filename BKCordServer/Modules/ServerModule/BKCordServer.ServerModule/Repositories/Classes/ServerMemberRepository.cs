using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.ServerModule.Repositories.Classes;
public class ServerMemberRepository : IServerMemberRepository
{
    private AppServerDbContext _dbContext;

    public ServerMemberRepository(AppServerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ServerMember serverMember)
    {
        await _dbContext.ServerMembers.AddAsync(serverMember);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(ServerMember serverMember)
    {
        _dbContext.ServerMembers.Remove(serverMember);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ServerMember?> GetByUserIdAndServerId(Guid userId, Guid serverId) =>
        await _dbContext.ServerMembers.FirstOrDefaultAsync(sm => sm.UserId == userId && sm.ServerId == serverId);

    public async Task<IEnumerable<Guid>> GetServerIdsByUserIdAsync(Guid userId) =>
        await _dbContext.ServerMembers.Where(sm => sm.UserId == userId).Select(sm => sm.ServerId).ToListAsync();

    public async Task<IEnumerable<Guid>> GetUserIdsByServerIdAsync(Guid serverId) =>
        await _dbContext.ServerMembers.Where(sm => sm.ServerId == serverId).Select(sm => sm.UserId).ToListAsync();
}
