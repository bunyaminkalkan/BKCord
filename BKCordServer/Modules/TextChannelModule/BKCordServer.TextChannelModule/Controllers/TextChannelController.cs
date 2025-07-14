using BKCordServer.TextChannelModule.UseCases.TextChannel.CreateTextChannel;
using BKCordServer.TextChannelModule.UseCases.TextChannel.DeleteTextChannel;
using BKCordServer.TextChannelModule.UseCases.TextChannel.GetAllTextChannels;
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

    [HttpDelete("{TextChannelId}")]
    public async Task<IActionResult> DeleteTextChannel([FromRoute] DeleteTextChannelCommand request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTextChannels([FromQuery] GetAllTextChannelsQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
