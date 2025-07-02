using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.Repositories.Interfaces;
using BKCordServer.ServerModule.Services.Interfaces;
using Shared.Kernel.Exceptions;

namespace BKCordServer.ServerModule.Services.Classes;
public class ServerMembersHistoryService : IServerMembersHistoryService
{
    private readonly IServerMembersHistoryRepository _serverMembersHistoryRepository;

    public ServerMembersHistoryService(IServerMembersHistoryRepository serverMembersHistoryRepository)
    {
        _serverMembersHistoryRepository = serverMembersHistoryRepository;
    }

    public async Task JoinServerAsync(Guid userId, Guid serverId)
    {
        var now = DateTime.UtcNow;

        var serverMembersHistory = new ServerMembersHistory
        {
            UserId = userId,
            ServerId = serverId,
            JoinedAt = now,
            LeftAt = now
        };

        await _serverMembersHistoryRepository.AddAsync(serverMembersHistory);
    }

    public async Task LeftServerAsync(Guid userId, Guid serverId, bool wasBanned, string? removedReason, Guid? removedByUserId)
    {
        var serverMembersHistory = await _serverMembersHistoryRepository.GetByUserIdAndServerIdAsync(userId, serverId);

        if (serverMembersHistory == null)
            throw new NotFoundException($"History connot be find with {serverId} serverId and {userId} userId");

        serverMembersHistory!.LeftAt = DateTime.UtcNow;
        serverMembersHistory!.RemovedReason = removedReason;
        serverMembersHistory!.RemovedByUserId = removedByUserId;
        serverMembersHistory!.WasBanned = wasBanned;

        await _serverMembersHistoryRepository.UpdateAsync(serverMembersHistory!);
    }

    public async Task DeleteAllHistoriesAsync(Guid serverId) =>
        await _serverMembersHistoryRepository.DeleteAllByServerIdAsync(serverId);
}
