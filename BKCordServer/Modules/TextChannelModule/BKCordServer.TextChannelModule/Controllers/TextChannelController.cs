using BKCordServer.TextChannelModule.UseCases.TextChannel.CreateTextChannel;
using BKCordServer.TextChannelModule.UseCases.TextChannel.UpdateTextChannel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.TextChannelModule.Controllers;

[ApiController]
[Route("[controller]/")]
[Authorize]
public class TextChannelController : ControllerBase
{
    private readonly IMediator _mediator;

    public TextChannelController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTextChannel([FromBody] CreateTextChannelCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTextChannel([FromBody] UpdateTextChannelCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
