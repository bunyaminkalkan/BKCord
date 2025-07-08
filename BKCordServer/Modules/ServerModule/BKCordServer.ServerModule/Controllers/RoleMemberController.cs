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

    [HttpDelete("{userId}/{roleId}")]
    public async Task<IActionResult> DeleteRoleFromUserAsync([FromRoute] string userId, [FromRoute] string roleId)
    {
        var request = new DeleteRoleFromUserCommand(Guid.Parse(userId), Guid.Parse(roleId));
        await _mediator.Send(request);
        return Ok();
    }
}
