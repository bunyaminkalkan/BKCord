using BKCordServer.TextChannelModule.UseCases.TextMessage.SendTextMessage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BKCordServer.TextChannelModule.SignalR;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMediator _mediator;

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendMessage(Guid textChannelId, string content)
    {
        var userId = Guid.Parse(Context.User?.FindFirst("user_id")?.Value);

        var messageDTO = await _mediator.Send(new SendTextMessageCommand(userId, textChannelId, content));

        await Clients.Group(textChannelId.ToString())
                     .SendAsync("ReceiveMessage", messageDTO);
    }

    public async Task JoinChannel(Guid textChannelId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, textChannelId.ToString());
    }

    public async Task LeaveChannel(Guid textChannelId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, textChannelId.ToString());
    }
}

