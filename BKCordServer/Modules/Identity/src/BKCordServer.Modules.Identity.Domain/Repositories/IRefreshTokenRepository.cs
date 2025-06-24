using BKCordServer.Modules.Identity.Domain.Entities;

namespace BKCordServer.Modules.Identity.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
    Task DeleteAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByIdAsync(string id);
    Task<RefreshToken?> GetByUserIdAsync(string userId);
}
