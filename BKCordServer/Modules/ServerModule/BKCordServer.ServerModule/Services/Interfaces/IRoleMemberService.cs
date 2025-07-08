namespace BKCordServer.ServerModule.Services.Interfaces;
public interface IRoleMemberService
{
    Task AssignRoleToUserAsync(Guid userId, Guid roleId);
    Task DeleteRoleFromUserAsync(Guid userId, Guid roleId);
    Task DeleteAllMembersAsync(Guid roleId);
}
