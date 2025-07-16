using BKCordServer.IdentityModule.Contracts;
using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using BKCordServer.TextChannelModule.DTOs;
using BKCordServer.TextChannelModule.SignalR;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Services;

namespace BKCordServer.TextChannelModule.UseCases.TextMessage.UpdateTextMessage;
public class UpdateTextMessageHandler : IRequestHandler<UpdateTextMessageCommand, TextMessageDTO>
{
    private readonly AppTextChannelDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IHttpContextService _httpContextService;
    private readonly IHubContext<ChatHub> _chatHubContext;

    public UpdateTextMessageHandler(AppTextChannelDbContext dbContext, IMediator mediator, IHttpContextService httpContextService, IHubContext<ChatHub> chatHubContext)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _httpContextService = httpContextService;
        _chatHubContext = chatHubContext;
    }

    public async Task<TextMessageDTO> Handle(UpdateTextMessageCommand request, CancellationToken cancellationToken)
    {
        var textMessage = await _dbContext.TextMessages.FirstOrDefaultAsync(tm => tm.Id == request.TextMessageId)
            ?? throw new NotFoundException($"Message cannot be find with {request.TextMessageId} text message id");

        var userId = _httpContextService.GetUserId();

        if (textMessage.SenderUserId != userId)
            throw new ForbiddenException("Cannot edit others' messages");

        textMessage.Content = request.NewContent;
        textMessage.UpdatedAt = DateTime.UtcNow;

        _dbContext.TextMessages.Update(textMessage);
        await _dbContext.SaveChangesAsync();

        var userInf = await _mediator.Send(new GetUserInfQuery(userId));

        var updatedMessage = new TextMessageDTO(userInf, textMessage, true);

        await _chatHubContext.Clients.Group(textMessage.ChannelId.ToString())
            .SendAsync("MessageUpdated", updatedMessage);

        return updatedMessage;
    }
}
