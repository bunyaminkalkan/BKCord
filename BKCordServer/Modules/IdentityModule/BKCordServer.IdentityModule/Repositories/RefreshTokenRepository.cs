using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using BKCordServer.IdentityModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.IdentityModule.Repositories;

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

    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        _dbContext.Update(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(RefreshToken refreshToken) =>
        _dbContext.RefreshTokens.Remove(refreshToken);

    public async Task<RefreshToken?> GetByUserIdAsync(Guid userId) =>
        await _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.UserId == userId);

    public async Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken) =>
        await _dbContext.RefreshTokens.Include(r => r.User).FirstOrDefaultAsync(r => r.Token == refreshToken);
}
