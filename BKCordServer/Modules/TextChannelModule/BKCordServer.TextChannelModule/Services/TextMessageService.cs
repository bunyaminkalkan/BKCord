using BKCordServer.TextChannelModule.Domain.Entities;
using BKCordServer.TextChannelModule.Repositories;
using BKCordServer.TextChannelModule.UseCases.TextMessage.SendTextMessage;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;

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

    public async Task UpdateAsync(string newContent, TextMessage textMessage)
    {
        textMessage.Content = newContent;
        textMessage.UpdatedAt = DateTime.UtcNow;

        await _textMessageRepository.UpdateAsync(textMessage);
    }

    public async Task DeleteAsync(Guid userId, TextMessage textMessage)
    {
        textMessage.IsDeleted = true;
        textMessage.DeletedBy = userId;
        textMessage.DeletedAt = DateTime.UtcNow;

        await _textMessageRepository.UpdateAsync(textMessage);
    }

    public async Task<IEnumerable<TextMessage>> GetAllByChannelIdAsync(Guid textChannelId) =>
        await _textMessageRepository.GetAsQueryable().Where(tm => tm.ChannelId == textChannelId).ToListAsync();

    public async Task<TextMessage> GetByIdAsync(Guid textMessageId)
    {
        var textMessage = await _textMessageRepository.GetAsQueryable().FirstOrDefaultAsync(tm => tm.Id == textMessageId);

        if (textMessage == null)
            throw new NotFoundException($"Message cannot be find with {textMessageId} text message id");

        return textMessage;
    }
}
