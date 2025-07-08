using BKCordServer.ServerModule.UseCases.Role.CreateRole;
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
        await _mediator.Send(request);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleCommand request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpDelete("{RoleId}")]
    public async Task<IActionResult> DeleteRoleAsync([FromRoute] string RoleId)
    {
        var request = new DeleteRoleCommand(Guid.Parse(RoleId));
        await _mediator.Send(request);
        return Ok();
    }

    [HttpGet("{ServerId}")]
    public async Task<IActionResult> GetServerRolesAsync([FromRoute] string ServerId)
    {
        var request = new GetServerRolesQuery(Guid.Parse(ServerId));
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
