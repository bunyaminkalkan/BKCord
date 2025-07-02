using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.Repositories.Interfaces;
using BKCordServer.ServerModule.Services.Interfaces;
using Shared.Kernel.Exceptions;

namespace BKCordServer.ServerModule.Services.Classes;
public class ServerMemberService : IServerMemberService
{
    private readonly IServerMemberRepository _serverMemberRepository;

    public ServerMemberService(IServerMemberRepository serverMemberRepository)
    {
        _serverMemberRepository = serverMemberRepository;
    }

    public async Task JoinServerAsync(Guid userId, Guid serverId)
    {
        var serverMember = new ServerMember
        {
            UserId = userId,
            ServerId = serverId
        };

        await _serverMemberRepository.AddAsync(serverMember);
    }

    public async Task LeftServerAsync(Guid userId, Guid serverId)
    {
        var serverMember = await _serverMemberRepository.GetByUserIdAndServerId(userId, serverId);

        if (serverMember == null)
            throw new NotFoundException($"Server member connot be find {userId} userId and {serverId} serverId");

        await _serverMemberRepository.DeleteAsync(serverMember);
    }
}
