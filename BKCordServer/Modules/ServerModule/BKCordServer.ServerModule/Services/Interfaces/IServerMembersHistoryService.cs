namespace BKCordServer.ServerModule.Services.Interfaces;
public interface IServerMembersHistoryService
{
    Task JoinServerAsync(Guid userId, Guid serverId);
    Task LeftServerAsync(Guid userId, Guid serverId, bool wasBanned, string? removedReason, Guid? removedByUserId);
    Task DeleteAllHistoriesAsync(Guid serverId);
}
