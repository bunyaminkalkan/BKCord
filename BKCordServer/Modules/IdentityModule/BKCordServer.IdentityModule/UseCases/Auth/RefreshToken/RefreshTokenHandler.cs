using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using BKCordServer.IdentityModule.DTOs;
using BKCordServer.IdentityModule.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;

namespace BKCordServer.IdentityModule.UseCases.Auth.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, JwtResponse>
{
    private readonly IJwtService _jwtService;
    private readonly AppIdentityDbContext _appIdentityDbContext;

    public RefreshTokenHandler(IJwtService jwtService, AppIdentityDbContext appIdentityDbContext)
    {
        _jwtService = jwtService;
        _appIdentityDbContext = appIdentityDbContext;
    }

    public async Task<JwtResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _appIdentityDbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == request.RefreshToken)
            ?? throw new BadRequestException("Enter a valid refresh token");

        var newTokens = await _jwtService.CreateTokenAsync(refreshToken.User);

        refreshToken.Token = newTokens.RefreshToken;
        refreshToken.Expires = DateTime.UtcNow.AddMonths(1);

        _appIdentityDbContext.RefreshTokens.Update(refreshToken);
        await _appIdentityDbContext.SaveChangesAsync();

        return newTokens;
    }
}
