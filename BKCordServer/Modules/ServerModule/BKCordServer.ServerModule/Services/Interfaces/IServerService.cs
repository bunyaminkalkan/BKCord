using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.Services.Interfaces;
public interface IServerService
{
    Task CreateAsync(Guid userId, string Name, string LogoUrl);
    Task<Guid> GetServerIdByInviteCodeAsync(string inviteCode);
    Task<IEnumerable<Server>> GetAllByIdsAsync(IEnumerable<Guid> ids);
}
