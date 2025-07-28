using BKCordServer.IdentityModule.UseCases.Auth.ConfirmEmail;
using BKCordServer.IdentityModule.UseCases.Auth.DisableTwoFactor;
using BKCordServer.IdentityModule.UseCases.Auth.EnableTwoFactor;
using BKCordServer.IdentityModule.UseCases.Auth.ForgotPassword;
using BKCordServer.IdentityModule.UseCases.Auth.GenerateTwoFactorRecoveryCodes;
using BKCordServer.IdentityModule.UseCases.Auth.GetTwoFactorStatus;
using BKCordServer.IdentityModule.UseCases.Auth.Login;
using BKCordServer.IdentityModule.UseCases.Auth.RefreshToken;
using BKCordServer.IdentityModule.UseCases.Auth.Register;
using BKCordServer.IdentityModule.UseCases.Auth.ResendEmailConfirmation;
using BKCordServer.IdentityModule.UseCases.Auth.ResetPassword;
using BKCordServer.IdentityModule.UseCases.Auth.SendTwoFactorVerificationCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BKCordServer.IdentityModule.Controllers;

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
        return Ok(new { message = "Registration successful! Please confirm your email address." });
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand request)
    {
        await _mediator.Send(request);
        return Ok(new { message = "Your email address has been successfully confirmed! You can now log in." });
    }

    [HttpPost("resend-email-confirmation")]
    public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailConfirmationCommand request)
    {
        await _mediator.Send(request);
        return Ok(new { message = "Confirmation email resent." });
    }

    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("forgot-password")]
    [EnableRateLimiting("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand request)
    {
        await _mediator.Send(request);
        return Ok(new { message = "Password reset email has been sent, please check your email address." });
    }

    [HttpPost("reset-password")]
    [EnableRateLimiting("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request)
    {
        await _mediator.Send(request);
        return Ok(new { message = "Your password has been updated successfully." });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    //2fa
    [HttpGet("2fa/status")]
    [Authorize]
    public async Task<IActionResult> GetTwoFactorStatus()
    {
        var request = new GetTwoFactorStatusQuery();
        var response = await _mediator.Send(request);
        return Ok(new { IsEnabled = response });
    }

    [HttpPost("2fa/send-code")]
    [Authorize]
    public async Task<IActionResult> SendTwoFactorVerificationCode()
    {
        var request = new SendTwoFactorVerificationCodeCommand();
        await _mediator.Send(request);
        return Ok(new { Message = "Verification code has been sent to your email address." });
    }

    [HttpPost("2fa/enable")]
    [Authorize]
    public async Task<IActionResult> EnableTwoFactor([FromBody] EnableTwoFactorCommand request)
    {
        var response = await _mediator.Send(request);
        return Ok(new
        {
            Message = "Two-factor authentication has been successfully enabled.",
            RecoveryCodes = response
        });
    }

    [HttpPost("2fa/disable")]
    [Authorize]
    public async Task<IActionResult> DisableTwoFactor()
    {
        var request = new DisableTwoFactorCommand();
        await _mediator.Send(request);
        return Ok(new { Message = "Two-factor authentication has been disabled." });
    }

    [HttpPost("2fa/generate-recovery-codes")]
    [Authorize]
    public async Task<IActionResult> GenerateTwoFactorRecoveryCodes()
    {
        var request = new GenerateTwoFactorRecoveryCodesCommand();
        var response = await _mediator.Send(request);
        return Ok(new
        {
            Message = "New recovery codes have been generated and sent to your email address.",
            RecoveryCodes = response
        });
    }
}
