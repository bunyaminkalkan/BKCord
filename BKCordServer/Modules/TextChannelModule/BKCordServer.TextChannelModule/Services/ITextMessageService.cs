using BKCordServer.TextChannelModule.Domain.Entities;
using BKCordServer.TextChannelModule.UseCases.TextMessage.SendTextMessage;

namespace BKCordServer.TextChannelModule.Services;
public interface ITextMessageService
{
    Task<TextMessage> CreateAsync(Guid userId, SendTextMessageCommand request);
    Task<IEnumerable<TextMessage>> GetAllByChannelIdAsync(Guid textChannelId);
}
