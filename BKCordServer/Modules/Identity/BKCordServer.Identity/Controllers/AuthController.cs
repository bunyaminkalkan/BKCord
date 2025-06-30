using BKCordServer.Identity.UseCases.Auth.Login;
using BKCordServer.Identity.UseCases.Auth.RefreshToken;
using BKCordServer.Identity.UseCases.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.Identity.Controllers;

[ApiController]
[Route("[controller]/")]
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
