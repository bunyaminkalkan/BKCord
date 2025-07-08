namespace BKCordServer.ServerModule.Services.Interfaces;
public interface IRoleMemberService
{
    Task AssignRoleToUserAsync(Guid userId, Guid roleId);
    Task DeleteRoleFromUserAsync(Guid userId, Guid roleId);
    Task DeleteAllMembersAsync(Guid roleId);
    Task<IEnumerable<Guid>> GetUserIdsByRoleIdAsync(Guid roleId);
    Task<IEnumerable<Guid>> GetRoleIdsByUserIdAsync(Guid userId);
}
