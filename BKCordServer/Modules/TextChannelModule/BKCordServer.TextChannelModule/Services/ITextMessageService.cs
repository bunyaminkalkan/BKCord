using BKCordServer.TextChannelModule.Domain.Entities;
using BKCordServer.TextChannelModule.UseCases.TextMessage.SendTextMessage;

namespace BKCordServer.TextChannelModule.Services;
public interface ITextMessageService
{
    Task<TextMessage> CreateAsync(Guid userId, SendTextMessageCommand request);
    Task UpdateAsync(string newContent, TextMessage textMessage);
    Task<TextMessage> GetByIdAsync(Guid textMessageId);
    Task<IEnumerable<TextMessage>> GetAllByChannelIdAsync(Guid textChannelId);
}
