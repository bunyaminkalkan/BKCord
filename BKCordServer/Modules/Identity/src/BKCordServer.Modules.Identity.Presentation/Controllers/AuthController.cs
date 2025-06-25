using BKCordServer.Modules.Identity.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.Modules.Identity.Presentation.Controllers;

[ApiController]
[Route("identity/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        await _mediator.Send(request);
        return Ok();
    }
}
