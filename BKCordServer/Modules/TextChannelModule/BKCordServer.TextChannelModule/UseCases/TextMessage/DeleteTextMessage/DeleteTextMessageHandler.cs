using BKCordServer.ServerModule.Contracts;
using BKCordServer.TextChannelModule.Services;
using BKCordServer.TextChannelModule.SignalR;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.DeleteTextMessage;
public class DeleteTextMessageHandler : IRequestHandler<DeleteTextMessageCommand>
{
    private readonly ITextMessageService _textMessageService;
    private readonly ITextChannelService _textChannelService;
    private readonly IHttpContextService _httpContextService;
    private readonly IHubContext<ChatHub> _chatHubContext;
    private readonly IMediator _mediator;

    public DeleteTextMessageHandler(
        ITextMessageService textMessageService,
        ITextChannelService textChannelService,
        IHttpContextService httpContextService,
        IHubContext<ChatHub> chatHubContext,
        IMediator mediator)
    {
        _textMessageService = textMessageService;
        _textChannelService = textChannelService;
        _httpContextService = httpContextService;
        _chatHubContext = chatHubContext;
        _mediator = mediator;
    }

    public async Task Handle(DeleteTextMessageCommand request, CancellationToken cancellationToken)
    {
        var textMessage = await _textMessageService.GetByIdAsync(request.TextMessageId);

        var userId = _httpContextService.GetUserId();
        var serverId = (await _textChannelService.GetByIdAsync(textMessage.ChannelId)).ServerId;

        var isOwnMessage = textMessage.SenderUserId == userId;
        var isModerator = await _mediator.Send(new IsUserHavePermissionQuery(userId, serverId, RolePermission.ManageMessages));

        if (!isOwnMessage && !isModerator)
            throw new ForbiddenException("You cannot delete other users' messages.");

        await _textMessageService.DeleteAsync(userId, textMessage);

        await _chatHubContext.Clients.Group(textMessage.ChannelId.ToString())
            .SendAsync("MessageDeleted", textMessage.Id);
    }
}
