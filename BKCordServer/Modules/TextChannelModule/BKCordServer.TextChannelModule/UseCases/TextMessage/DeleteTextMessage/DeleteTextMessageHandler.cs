using BKCordServer.ServerModule.Contracts;
using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using BKCordServer.TextChannelModule.SignalR;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.DeleteTextMessage;
public class DeleteTextMessageHandler : IRequestHandler<DeleteTextMessageCommand>
{
    private readonly AppTextChannelDbContext _dbContext;
    private readonly IHttpContextService _httpContextService;
    private readonly IHubContext<ChatHub> _chatHubContext;
    private readonly IMediator _mediator;

    public DeleteTextMessageHandler(
        AppTextChannelDbContext dbContext,
        IHttpContextService httpContextService,
        IHubContext<ChatHub> chatHubContext,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _httpContextService = httpContextService;
        _chatHubContext = chatHubContext;
        _mediator = mediator;
    }

    public async Task Handle(DeleteTextMessageCommand request, CancellationToken cancellationToken)
    {
        var textMessage = await _dbContext.TextMessages.FirstOrDefaultAsync(tm => tm.Id == request.TextMessageId)
            ?? throw new NotFoundException($"Message cannot be find with {request.TextMessageId} text message id");

        var textChannel = await _dbContext.TextChannels.FirstOrDefaultAsync(tc => tc.Id == textMessage.ChannelId)
            ?? throw new NotFoundException($"Text Channel cannot find with {textMessage.ChannelId} text channel id");

        var userId = _httpContextService.GetUserId();
        var serverId = textChannel.ServerId;

        var isOwnMessage = textMessage.SenderUserId == userId;
        var isModerator = await _mediator.Send(new IsUserHavePermissionQuery(userId, serverId, RolePermission.ManageMessages));

        if (!isOwnMessage && !isModerator)
            throw new ForbiddenException("You cannot delete other users' messages.");

        textMessage.IsDeleted = true;
        textMessage.DeletedBy = userId;
        textMessage.DeletedAt = DateTime.UtcNow;

        textChannel.MessageCount--;

        _dbContext.TextMessages.Update(textMessage);
        _dbContext.TextChannels.Update(textChannel);
        await _dbContext.SaveChangesAsync();

        await _chatHubContext.Clients.Group(textMessage.ChannelId.ToString())
            .SendAsync("MessageDeleted", textMessage.Id);
    }
}
