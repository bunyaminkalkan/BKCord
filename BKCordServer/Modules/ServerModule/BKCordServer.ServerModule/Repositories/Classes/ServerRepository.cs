using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.ServerModule.Repositories.Classes;

public class ServerRepository : IServerRepository
{
    private readonly AppServerDbContext _dbContext;

    public ServerRepository(AppServerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Server server)
    {
        await _dbContext.Servers.AddAsync(server);
        await _dbContext.SaveChangesAsync();
    }
    public async Task UpdateAsync(Server server)
    {
        _dbContext.Servers.Update(server);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Server server)
    {
        _dbContext.Servers.Remove(server);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Server?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Servers.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Server>> GetAllByIdsAsync(IEnumerable<Guid> ids) =>
        await _dbContext.Servers.Where(s => ids.Contains(s.Id)).ToListAsync();

    public async Task<Guid?> GetServerIdByInviteCodeAsync(string inviteCode) =>
        (await _dbContext.Servers.FirstOrDefaultAsync(s => s.InviteCode == inviteCode))?.Id;
}
