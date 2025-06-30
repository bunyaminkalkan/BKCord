using BKCordServer.Identity.Domain.Entities;

namespace BKCordServer.Identity.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
    Task DeleteAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByUserIdAsync(string userId);
    Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken);
}
