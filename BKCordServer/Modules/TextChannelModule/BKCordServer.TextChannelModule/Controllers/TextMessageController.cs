using BKCordServer.TextChannelModule.UseCases.TextMessage.GetAllTextChannelMessages;
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

    [HttpGet]
    public async Task<IActionResult> GetAllTextChannelMessages([FromQuery] GetAllTextChannelMessagesQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
