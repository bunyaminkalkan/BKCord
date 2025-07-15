using BKCordServer.TextChannelModule.Domain.Entities;
using BKCordServer.TextChannelModule.Repositories;
using BKCordServer.TextChannelModule.UseCases.TextMessage.SendTextMessage;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.TextChannelModule.Services;
public class TextMessageService : ITextMessageService
{
    private readonly ITextMessageRepository _textMessageRepository;

    public TextMessageService(ITextMessageRepository textMessageRepository)
    {
        _textMessageRepository = textMessageRepository;
    }

    public async Task<TextMessage> CreateAsync(Guid userId, SendTextMessageCommand request)
    {
        var textMessage = new TextMessage
        {
            SenderUserId = userId,
            ChannelId = request.TextChannelId,
            Content = request.Content,
        };

        await _textMessageRepository.AddAsync(textMessage);

        return textMessage;
    }

    public async Task<IEnumerable<TextMessage>> GetAllByChannelIdAsync(Guid textChannelId) =>
        await _textMessageRepository.GetAsQueryable().Where(tm => tm.ChannelId == textChannelId).ToListAsync();
}
