using BKCordServer.IdentityModule.Domain.Entities;
using BKCordServer.IdentityModule.Domain.Enums;
using BKCordServer.IdentityModule.DTOs;
using BKCordServer.IdentityModule.Repositories;
using BKCordServer.IdentityModule.UseCases.Auth.Login;
using BKCordServer.IdentityModule.UseCases.Auth.Register;
using Microsoft.AspNetCore.Identity;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(UserManager<User> userManager, IJwtService jwtService, IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<JwtResponse> LoginAsync(LoginCommand request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new BadRequestException("Invalid email or password");
        }

        var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isValidPassword)
        {
            throw new BadRequestException("Invalid email or password");
        }

        var tokens = await _jwtService.CreateTokenAsync(user);

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = tokens.RefreshToken
        };

        await _refreshTokenRepository.AddAsync(refreshToken);

        return tokens;
    }

    public async Task RegisterAsync(RegisterCommand request)
    {
        var now = DateTime.UtcNow;

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Email = request.Email,
            Name = request.Name,
            Middlename = request.Middlename,
            Surname = request.Surname,
            AvatarUrl = "",
            Status = UserStatus.Active,
            IsPrivateAccount = false,
            EmailConfirmed = false,
            CreatedAt = now,
            UpdatedAt = now,
            DeletedAt = now,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"Register failed: {errors}");
        }
    }
}
