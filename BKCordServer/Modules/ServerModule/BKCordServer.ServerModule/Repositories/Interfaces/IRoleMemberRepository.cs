using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.Repositories.Interfaces;
public interface IRoleMemberRepository
{
    Task AddAsync(RoleMember roleMember);
    Task DeleteAsync(RoleMember roleMember);
    Task<IEnumerable<Guid>> GetRoleIdsByUserIdAndServerIdAsync(Guid userId, Guid serverId);
}
