using BKCordServer.Modules.Identity.Application.DTOs;
using BKCordServer.Modules.Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.Modules.Identity.Presentation.Controllers;

[ApiController]
[Route("identity/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        await _authService.RegisterAsync(dto.Email, dto.UserName, dto.Password);
        return Ok();
    }
}
