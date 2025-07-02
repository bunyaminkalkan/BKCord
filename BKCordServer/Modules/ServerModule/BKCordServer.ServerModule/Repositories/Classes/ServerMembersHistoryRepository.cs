using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.ServerModule.Repositories.Classes;
public class ServerMembersHistoryRepository : IServerMembersHistoryRepository
{
    private readonly AppServerDbContext _dbContext;

    public ServerMembersHistoryRepository(AppServerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ServerMembersHistory serverMembersHistory)
    {
        await _dbContext.ServerMembersHistory.AddAsync(serverMembersHistory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(ServerMembersHistory serverMembersHistory)
    {
        _dbContext.ServerMembersHistory.Update(serverMembersHistory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(ServerMembersHistory serverMembersHistory)
    {
        _dbContext.ServerMembersHistory.Remove(serverMembersHistory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ServerMembersHistory?> GetByUserIdAndServerIdAsync(Guid userId, Guid serverId) =>
        await _dbContext.ServerMembersHistory.FirstOrDefaultAsync(smh => smh.UserId == userId && smh.ServerId == serverId);
}
