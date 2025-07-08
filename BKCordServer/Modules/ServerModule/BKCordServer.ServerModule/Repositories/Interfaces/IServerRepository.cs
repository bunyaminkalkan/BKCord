using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.Repositories.Interfaces;
public interface IServerRepository
{
    Task AddAsync(Server server);
    Task UpdateAsync(Server server);
    Task<Server?> GetByIdAsync(Guid id);
    Task<IEnumerable<Server>> GetAllByIdsAsync(IEnumerable<Guid> ids);
    Task<Guid?> GetServerIdByInviteCodeAsync(string inviteCode);
}
