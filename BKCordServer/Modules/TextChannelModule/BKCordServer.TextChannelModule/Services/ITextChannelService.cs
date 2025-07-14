using BKCordServer.TextChannelModule.Domain.Entities;
using BKCordServer.TextChannelModule.UseCases.TextChannel.CreateTextChannel;

namespace BKCordServer.TextChannelModule.Services;
public interface ITextChannelService
{
    Task<TextChannel> CreateAsync(Guid createdUserId, CreateTextChannelCommand request);
}
