using BKCordServer.Modules.Identity.Domain.Entities;
using BKCordServer.Modules.Identity.Domain.Repositories;

namespace BKCordServer.Modules.Identity.Infrastructure.Persistance.Repositories;
public class RefreshTokenRepository : IRefreshTokenRepository
{
    public Task AddAsync(RefreshToken refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(RefreshToken refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken?> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken?> GetByUserIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(RefreshToken refreshToken)
    {
        throw new NotImplementedException();
    }
}
