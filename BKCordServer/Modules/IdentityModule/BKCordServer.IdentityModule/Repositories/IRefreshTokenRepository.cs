using BKCordServer.IdentityModule.Domain.Entities;

namespace BKCordServer.IdentityModule.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
    Task DeleteAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByUserIdAsync(Guid userId);
    Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken);
}
