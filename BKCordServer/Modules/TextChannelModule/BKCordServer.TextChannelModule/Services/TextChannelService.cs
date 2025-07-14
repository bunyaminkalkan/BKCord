using BKCordServer.TextChannelModule.Domain.Entities;
using BKCordServer.TextChannelModule.Repositories;
using BKCordServer.TextChannelModule.UseCases.TextChannel.CreateTextChannel;
using BKCordServer.TextChannelModule.UseCases.TextChannel.UpdateTextChannel;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;

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

    public async Task<TextChannel> UpdateAsync(Guid updatedUserId, UpdateTextChannelCommand request, TextChannel textChannel)
    {
        textChannel.UpdatedBy = updatedUserId;
        textChannel.UpdatedAt = DateTime.UtcNow;

        await _textChannelRepository.UpdateAsync(textChannel);

        return textChannel;
    }

    public async Task<TextChannel> GetByIdAsync(Guid textChannelId)
    {
        var textChannel = await _textChannelRepository.GetAsQueryable().FirstOrDefaultAsync(tc => tc.Id == textChannelId);

        if (textChannel == null)
            throw new NotFoundException($"Text Channel cannot find with {textChannelId} text channel id");

        return textChannel;
    }
}
