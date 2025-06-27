using BKCordServer.Identity.UseCases.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.Identity.Controllers;

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
    public async Task<IActionResult> Register([FromBody] RegisterCommand request)
    {
        await _mediator.Send(request);
        return Ok();
    }
}
