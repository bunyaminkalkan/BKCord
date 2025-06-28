using BKCordServer.Identity.Data.Context.PostgreSQL;
using BKCordServer.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.Identity.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppIdentityDbContext _dbContext;

    public RefreshTokenRepository(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(RefreshToken refreshToken) =>
        _dbContext.RefreshTokens.Remove(refreshToken);

    public async Task<RefreshToken?> GetByUserIdAsync(string userId) =>
        await _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.UserId == userId);
}
