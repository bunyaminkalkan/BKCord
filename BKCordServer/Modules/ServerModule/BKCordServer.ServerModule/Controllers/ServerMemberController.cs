﻿using BKCordServer.ServerModule.UseCases.ServerMember.BanUser;
using BKCordServer.ServerModule.UseCases.ServerMember.GetServersByMemberUser;
using BKCordServer.ServerModule.UseCases.ServerMember.GetServerUsers;
using BKCordServer.ServerModule.UseCases.ServerMember.JoinServer;
using BKCordServer.ServerModule.UseCases.ServerMember.KickUser;
using BKCordServer.ServerModule.UseCases.ServerMember.LeftServer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.ServerModule.Controllers;

[ApiController]
[Route("[controller]/")]
[Authorize]
public class ServerMemberController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServerMemberController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinServerAsync([FromBody] JoinServerCommand request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpPost("left/{ServerId}")]
    public async Task<IActionResult> LeftServerAsync([FromRoute] LeftServerCommand request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpGet("listServers")]
    public async Task<IActionResult> ListServersByMemberUserAsync()
    {
        var request = new GetServersByMemberUserQuery();
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("listServerUsers/{ServerId}")]
    public async Task<IActionResult> ListServerUsersByMemberUserAsync([FromRoute] GetServerUsersQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("ban-user")]
    public async Task<IActionResult> BanUser([FromBody] BanUserCommand request)
    {
        await _mediator.Send(request);
        return Ok(new { message = "User was successfully banned from the server." });
    }

    [HttpPost("kick-user")]
    public async Task<IActionResult> KickUser([FromBody] KickUserCommand request)
    {
        await _mediator.Send(request);
        return Ok(new { message = "User was successfully kicked from the server." });
    }
}
