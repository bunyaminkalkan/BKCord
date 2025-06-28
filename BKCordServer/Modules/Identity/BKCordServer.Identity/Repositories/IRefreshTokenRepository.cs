using BKCordServer.Identity.Domain.Entities;

namespace BKCordServer.Identity.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task DeleteAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByUserIdAsync(string userId);
}
