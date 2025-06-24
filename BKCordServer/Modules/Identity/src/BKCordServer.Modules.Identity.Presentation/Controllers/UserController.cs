using BKCordServer.Modules.Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BKCordServer.Modules.Identity.Presentation.Controllers;

[ApiController]
[Route("identity/user/")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("getByEmail/{email}")]
    public async Task<IActionResult> GetByEmailAsync([FromRoute] string email)
    {
        var response = await _userService.GetByEmailAsync(email);
        return Ok(response);
    }
}
