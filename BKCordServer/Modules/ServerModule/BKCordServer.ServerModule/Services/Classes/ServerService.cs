using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.Repositories.Interfaces;
using BKCordServer.ServerModule.Services.Interfaces;
using Shared.Kernel.Exceptions;

namespace BKCordServer.ServerModule.Services.Classes;
public class ServerService : IServerService
{
    private readonly IServerRepository _serverRepository;

    public ServerService(IServerRepository serverRepository)
    {
        _serverRepository = serverRepository;
    }

    public async Task CreateAsync(Guid userId, string name, string logoUrl)
    {
        var now = DateTime.UtcNow;

        var server = new Server
        {
            OwnerId = userId,
            Name = name,
            LogoUrl = logoUrl,
            InviteCode = GenerateInviteCode(),
            CreatedAt = now,
            UpdatedAt = now,
        };

        await _serverRepository.AddAsync(server);
    }

    public async Task<Guid> GetServerIdByInviteCodeAsync(string inviteCode)
    {
        var serverId = await _serverRepository.GetServerIdByInviteCodeAsync(inviteCode);

        if (serverId == null)
            throw new NotFoundException($"Server connot find with {inviteCode} invite code");

        return serverId!.Value;
    }

    public async Task<IEnumerable<Server>> GetAllByIdsAsync(IEnumerable<Guid> ids) =>
        await _serverRepository.GetAllByIdsAsync(ids);

    private string GenerateInviteCode(int length = 15)
    {
        var guid = Guid.NewGuid();
        var base64 = Convert.ToBase64String(guid.ToByteArray());

        // Base64 string'i URL-safe hale getir (isteğe bağlı)
        var code = base64.Replace("+", "")
                         .Replace("/", "")
                         .Replace("=", "");

        return code.Substring(0, Math.Min(length, code.Length));
    }
}
