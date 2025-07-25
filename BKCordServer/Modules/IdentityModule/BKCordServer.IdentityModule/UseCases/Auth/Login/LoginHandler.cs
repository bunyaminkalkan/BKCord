using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using BKCordServer.IdentityModule.DTOs;
using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.Login;

public class LoginHandler : IRequestHandler<LoginCommand, JwtResponse>
{
    private readonly UserManager<Domain.Entities.User> _userManager;
    private readonly IJwtService _jwtService;
    private readonly AppIdentityDbContext _appIdentityDbContext;

    public LoginHandler(UserManager<Domain.Entities.User> userManager, IJwtService jwtService, AppIdentityDbContext appIdentityDbContext)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _appIdentityDbContext = appIdentityDbContext;
    }

    public async Task<JwtResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new BadRequestException("Invalid email or password");

        if (!user.EmailConfirmed)
            throw new BadRequestException("Your user email address is not verified. You must verify your email address to log in.");

        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isValidPassword)
            throw new BadRequestException("Invalid email or password");

        var tokens = await _jwtService.CreateTokenAsync(user);

        var refreshToken = new Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            Token = tokens.RefreshToken
        };

        _appIdentityDbContext.RefreshTokens.Add(refreshToken);

        await _appIdentityDbContext.SaveChangesAsync();

        return tokens;
    }
}
