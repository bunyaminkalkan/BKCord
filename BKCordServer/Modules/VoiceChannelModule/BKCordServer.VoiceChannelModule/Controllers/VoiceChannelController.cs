using BKCordServer.VoiceChannelModule.UseCases.CreateVoiceChannel;
using BKCordServer.VoiceChannelModule.UseCases.DeleteVoiceChannel;
using BKCordServer.VoiceChannelModule.UseCases.GetAllVoiceChannels;
using BKCordServer.VoiceChannelModule.UseCases.UpdateVoiceChannel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.VoiceChannelModule.Controllers;

[ApiController]
[Route("[controller]/")]
[Authorize]
public class VoiceChannelController : ControllerBase
{
    private readonly IMediator _mediator;

    public VoiceChannelController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateVoiceChannel([FromBody] CreateVoiceChannelCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPut("{VoiceChannelId}")]
    public async Task<IActionResult> UpdateVoiceChannel([FromBody] UpdateVoiceChannelCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete("{VoiceChannelId}")]
    public async Task<IActionResult> DeleteVoiceChannel([FromRoute] DeleteVoiceChannelCommand request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVoiceChannels([FromQuery] GetAllVoiceChannelsQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
