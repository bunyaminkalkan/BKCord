using BKCordServer.ServerModule.UseCases.Role.CreateRole;
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
}
