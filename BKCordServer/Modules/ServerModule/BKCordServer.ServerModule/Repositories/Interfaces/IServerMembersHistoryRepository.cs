using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.Repositories.Interfaces;
public interface IServerMembersHistoryRepository
{
    Task AddAsync(ServerMembersHistory serverMembersHistory);
    Task UpdateAsync(ServerMembersHistory serverMembersHistory);
    Task DeleteAsync(ServerMembersHistory serverMembersHistory);
    Task DeleteAllByServerIdAsync(Guid serverId);
    Task<ServerMembersHistory?> GetByUserIdAndServerIdAsync(Guid userId, Guid serverId);
}
