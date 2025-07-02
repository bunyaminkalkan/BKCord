namespace BKCordServer.ServerModule.Services.Interfaces;
public interface IServerMemberService
{
    Task JoinServerAsync(Guid userId, Guid serverId);
    Task LeftServerAsync(Guid userId, Guid serverId);
    Task<IEnumerable<Guid>> GetServerIdsByUserIdAsync(Guid userId);
    Task ValidateMemberJoinedServer(Guid userId, Guid serverId);
}
