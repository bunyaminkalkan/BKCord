using BKCordServer.Identity.Domain.Entities;
using BKCordServer.Identity.DTOs;

namespace BKCordServer.Identity.Services;

public interface IJwtService
{
    Task<JwtResponse> CreateTokenAsync(User user);
}
