using BKCordServer.ServerModule.UseCases.RoleMember.AssignRoleToUser;
using BKCordServer.ServerModule.UseCases.RoleMember.DeleteRoleFromUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.ServerModule.Controllers;

[ApiController]
[Route("[controller]/")]
[Authorize]
public class RoleMemberController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleMemberController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AssignRoleToUserAsync([FromBody] AssignRoleToUserCommand request)
    {
        await _mediator.Send(request);
        return Ok();
    }

    [HttpDelete("{UserId}/{RoleId}")]
    public async Task<IActionResult> DeleteRoleFromUserAsync([FromRoute] DeleteRoleFromUserCommand request)
    {
        await _mediator.Send(request);
        return Ok();
    }
}
