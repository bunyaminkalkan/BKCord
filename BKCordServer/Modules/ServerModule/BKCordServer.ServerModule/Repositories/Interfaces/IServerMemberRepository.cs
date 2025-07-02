using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.Repositories.Interfaces;
public interface IServerMemberRepository
{
    Task AddAsync(ServerMember serverMember);
    Task DeleteAsync(ServerMember serverMember);
    Task<ServerMember?> GetByUserIdAndServerId(Guid userId, Guid serverId);
    Task<IEnumerable<Guid>> GetServerIdsByUserIdAsync(Guid userId);
    Task<IEnumerable<Guid>> GetUserIdsByServerIdAsync(Guid serverId);
    Task<bool> ExistAsync(Guid userId, Guid serverId);
}
