using BKCordServer.IdentityModule.DTOs;
using BKCordServer.IdentityModule.Repositories;
using BKCordServer.IdentityModule.Services;
using MediatR;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, JwtResponse>
{
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenHandler(IJwtService jwtService, IRefreshTokenRepository refreshTokenRepository)
    {
        _jwtService = jwtService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<JwtResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByRefreshTokenAsync(request.RefreshToken);

        if (refreshToken == null)
            throw new BadRequestException("Enter a valid refresh token");

        var tokens = await _jwtService.CreateTokenAsync(refreshToken.User);

        refreshToken.Token = tokens.RefreshToken;
        refreshToken.Expires = DateTime.UtcNow.AddMonths(1);

        await _refreshTokenRepository.UpdateAsync(refreshToken);

        return tokens;
    }
}
