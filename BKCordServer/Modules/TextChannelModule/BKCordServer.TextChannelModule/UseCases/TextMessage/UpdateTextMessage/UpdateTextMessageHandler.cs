using BKCordServer.IdentityModule.Contracts;
using BKCordServer.TextChannelModule.DTOs;
using BKCordServer.TextChannelModule.Services;
using BKCordServer.TextChannelModule.SignalR;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.UpdateTextMessage;
public class UpdateTextMessageHandler : IRequestHandler<UpdateTextMessageCommand, TextMessageDTO>
{
    private readonly ITextMessageService _textMessageService;
    private readonly IHttpContextService _httpContextService;
    private readonly IMediator _mediator;
    private readonly IHubContext<ChatHub> _chatHubContext;

    public UpdateTextMessageHandler(ITextMessageService textMessageService, IHttpContextService httpContextService, IMediator mediator, IHubContext<ChatHub> chatHubContext)
    {
        _textMessageService = textMessageService;
        _httpContextService = httpContextService;
        _mediator = mediator;
        _chatHubContext = chatHubContext;
    }

    public async Task<TextMessageDTO> Handle(UpdateTextMessageCommand request, CancellationToken cancellationToken)
    {
        var textMessage = await _textMessageService.GetByIdAsync(request.TextMessageId);

        var userId = _httpContextService.GetUserId();

        if (textMessage.SenderUserId != userId)
            throw new ForbiddenException("Cannot edit others' messages");

        await _textMessageService.UpdateAsync(request.NewContent, textMessage);

        var userInf = await _mediator.Send(new GetUserInfQuery(userId));

        var updatedMessage = new TextMessageDTO(userInf, textMessage, true);

        await _chatHubContext.Clients.Group(textMessage.ChannelId.ToString())
            .SendAsync("MessageUpdated", updatedMessage);

        return updatedMessage;
    }
}
