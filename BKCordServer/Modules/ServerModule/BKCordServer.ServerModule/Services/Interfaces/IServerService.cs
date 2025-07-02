using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.Services.Interfaces;
public interface IServerService
{
    Task<Server> CreateAsync(Guid userId, string Name, string LogoUrl);
    Task<Server> GetByIdAsync(Guid serverId);
    Task<Guid> GetServerIdByInviteCodeAsync(string inviteCode);
    Task<IEnumerable<Server>> GetAllByIdsAsync(IEnumerable<Guid> ids);
}
