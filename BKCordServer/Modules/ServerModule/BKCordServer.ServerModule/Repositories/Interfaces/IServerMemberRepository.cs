using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.Repositories.Interfaces;
public interface IServerMemberRepository
{
    Task AddAsync(ServerMember serverMember);
    Task UpdateAsync(ServerMember serverMember);
    Task DeleteAsync(ServerMember serverMember);
    Task<IEnumerable<Guid>> GetServerIdsByUserIdAsync(Guid userId);
    Task<IEnumerable<Guid>> GetUserIdsByServerIdAsync(Guid serverId);
}
