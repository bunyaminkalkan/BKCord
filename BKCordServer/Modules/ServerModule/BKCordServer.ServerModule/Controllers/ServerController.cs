using BKCordServer.ServerModule.UseCases.Server.CreateServer;
using BKCordServer.ServerModule.UseCases.Server.GetServerInf;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.ServerModule.Controllers;

[ApiController]
[Route("[controller]/")]
[Authorize]
public class ServerController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateServerAsync([FromForm] CreateServerCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetServerInfAsync([FromQuery] GetServerInfQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
