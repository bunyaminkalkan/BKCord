using BKCordServer.TextChannelModule.Domain.Entities;
using BKCordServer.TextChannelModule.UseCases.TextChannel.CreateTextChannel;
using BKCordServer.TextChannelModule.UseCases.TextChannel.UpdateTextChannel;

namespace BKCordServer.TextChannelModule.Services;
public interface ITextChannelService
{
    Task<TextChannel> CreateAsync(Guid createdUserId, CreateTextChannelCommand request);
    Task<TextChannel> UpdateAsync(Guid updatedUserId, UpdateTextChannelCommand request, TextChannel textChannel);
    Task DeleteAsync(Guid deletedUserId, TextChannel textChannel);
    Task<TextChannel> GetByIdAsync(Guid textChannelId);
    Task<IEnumerable<TextChannel>> GetAllByServerIdAsync(Guid serverId);
}
