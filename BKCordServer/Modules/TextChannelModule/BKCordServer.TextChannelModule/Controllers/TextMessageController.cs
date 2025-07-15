using BKCordServer.TextChannelModule.DTOs;
using BKCordServer.TextChannelModule.UseCases.TextMessage.GetAllTextChannelMessages;
using BKCordServer.TextChannelModule.UseCases.TextMessage.UpdateTextMessage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.TextChannelModule.Controllers;

[ApiController]
[Route("[controller]/")]
public class TextMessageController : ControllerBase
{
    private readonly IMediator _mediator;

    public TextMessageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{TextMessageId}")]
    public async Task<IActionResult> UpdateTextMessage([FromRoute] Guid TextMessageId, UpdateTextMessageRequest request)
    {
        var command = new UpdateTextMessageCommand(TextMessageId, request.NewContent);
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTextChannelMessages([FromQuery] GetAllTextChannelMessagesQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
