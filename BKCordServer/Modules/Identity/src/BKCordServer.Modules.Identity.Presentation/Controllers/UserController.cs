using BKCordServer.Modules.Identity.Application.Features.Quries.GetByEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.Modules.Identity.Presentation.Controllers;

[ApiController]
[Route("identity/user/")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getByEmail/{email}")]
    public async Task<IActionResult> GetByEmailAsync([FromRoute] string email)
    {
        var request = new GetByEmailQuery(email);
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
