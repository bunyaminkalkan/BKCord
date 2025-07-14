using BKCordServer.TextChannelModule.Domain.Entities;
using BKCordServer.TextChannelModule.Repositories;
using BKCordServer.TextChannelModule.UseCases.TextChannel.CreateTextChannel;

namespace BKCordServer.TextChannelModule.Services;
public class TextChannelService : ITextChannelService
{
    private readonly ITextChannelRepository _textChannelRepository;

    public TextChannelService(ITextChannelRepository textChannelRepository)
    {
        _textChannelRepository = textChannelRepository;
    }

    public async Task<TextChannel> CreateAsync(Guid createdUserId, CreateTextChannelCommand request)
    {
        var textChannel = new TextChannel
        {
            ServerId = request.ServerId,
            CreatedBy = createdUserId,
            Name = request.Name
        };

        await _textChannelRepository.AddAsync(textChannel);

        return textChannel;
    }
}
