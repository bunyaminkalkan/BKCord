using BKCordServer.IdentityModule.Contracts;
using BKCordServer.TextChannelModule.DTOs;
using BKCordServer.TextChannelModule.Services;
using MediatR;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.SendTextMessage;
public class SendTextMessageHandler : IRequestHandler<SendTextMessageCommand, TextMessageDTO>
{
    private readonly ITextMessageService _textMessageService;
    private readonly ITextChannelService _textChannelService;
    private readonly IMediator _mediator;

    public SendTextMessageHandler(ITextMessageService textMessageService, ITextChannelService textChannelService, IMediator mediator)
    {
        _textMessageService = textMessageService;
        _textChannelService = textChannelService;
        _mediator = mediator;
    }

    public async Task<TextMessageDTO> Handle(SendTextMessageCommand request, CancellationToken cancellationToken)
    {
        var textChannel = await _textChannelService.GetByIdAsync(request.TextChannelId);

        var textMessage = await _textMessageService.CreateAsync(request.UserId, request);

        await _textChannelService.IncrementMessageCount(textChannel);

        var userInf = await _mediator.Send(new GetUserInfQuery(request.UserId));

        return new TextMessageDTO(userInf, textMessage, textMessage.CreatedAt != (textMessage.UpdatedAt ?? textMessage.CreatedAt));
    }
}
