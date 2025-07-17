using BKCordServer.IdentityModule.UseCases.User.GetByEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.IdentityModule.Controllers;

[ApiController]
[Route("[controller]/")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getByEmail/{Email}")]
    public async Task<IActionResult> GetByEmailAsync([FromRoute] GetByEmailQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
