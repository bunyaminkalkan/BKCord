using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.Repositories.Interfaces;
public interface IRoleMemberRepository
{
    Task AddAsync(RoleMember roleMember);
    Task DeleteAsync(RoleMember roleMember);
    Task DeleteAllMembersAsync(Guid roleId);
    Task<RoleMember?> GetByUserIdAndRoleIdAsync(Guid userId, Guid roleId);
    Task<IEnumerable<Guid>> GetRoleIdsByUserIdAsync(Guid userId);
    Task<IEnumerable<Guid>> GetUserIdsByRoleIdAsync(Guid roleId);
}
