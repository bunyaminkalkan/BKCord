﻿using BKCordServer.ServerModule.UseCases.Role.CreateRole;
using BKCordServer.ServerModule.UseCases.Role.DeleteRole;
using BKCordServer.ServerModule.UseCases.Role.GetServerRoles;
using BKCordServer.ServerModule.UseCases.Role.UpdateRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.ServerModule.Controllers;
[ApiController]
[Route("[controller]/")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete("{RoleId}")]
    public async Task<IActionResult> DeleteRoleAsync([FromRoute] DeleteRoleCommand request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpGet("{ServerId}")]
    public async Task<IActionResult> GetServerRolesAsync([FromRoute] GetServerRolesQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
