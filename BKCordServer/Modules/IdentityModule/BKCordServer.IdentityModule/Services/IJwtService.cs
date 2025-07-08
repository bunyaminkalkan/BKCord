using BKCordServer.IdentityModule.Domain.Entities;
using BKCordServer.IdentityModule.DTOs;

namespace BKCordServer.IdentityModule.Services;

public interface IJwtService
{
    Task<JwtResponse> CreateTokenAsync(User user);
}
