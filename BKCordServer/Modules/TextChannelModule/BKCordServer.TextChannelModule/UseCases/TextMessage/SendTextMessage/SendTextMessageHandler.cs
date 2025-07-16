using BKCordServer.IdentityModule.Contracts;
using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using BKCordServer.TextChannelModule.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.SendTextMessage;
public class SendTextMessageHandler : IRequestHandler<SendTextMessageCommand, TextMessageDTO>
{
    private readonly AppTextChannelDbContext _dbContext;
    private readonly IMediator _mediator;

    public SendTextMessageHandler(AppTextChannelDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<TextMessageDTO> Handle(SendTextMessageCommand request, CancellationToken cancellationToken)
    {
        var textChannel = await _dbContext.TextChannels.FirstOrDefaultAsync(tc => tc.Id == request.TextChannelId)
            ?? throw new NotFoundException($"Text Channel cannot find with {request.TextChannelId} text channel id");

        var textMessage = new Domain.Entities.TextMessage
        {
            SenderUserId = request.UserId,
            ChannelId = request.TextChannelId,
            Content = request.Content,
        };

        _dbContext.TextMessages.Add(textMessage);

        textChannel.MessageCount++;
        _dbContext.TextChannels.Update(textChannel);

        await _dbContext.SaveChangesAsync();

        var userInf = await _mediator.Send(new GetUserInfQuery(request.UserId));

        return new TextMessageDTO(userInf, textMessage, textMessage.CreatedAt != (textMessage.UpdatedAt ?? textMessage.CreatedAt));
    }
}
