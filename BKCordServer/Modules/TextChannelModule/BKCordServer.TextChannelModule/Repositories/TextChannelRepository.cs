using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using BKCordServer.TextChannelModule.Domain.Entities;

namespace BKCordServer.TextChannelModule.Repositories;
public class TextChannelRepository : ITextChannelRepository
{
    private readonly AppTextChannelDbContext _dbContext;

    public TextChannelRepository(AppTextChannelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TextChannel textChannel)
    {
        await _dbContext.TextChannels.AddAsync(textChannel);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TextChannel textChannel)
    {
        _dbContext.TextChannels.Update(textChannel);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(TextChannel textChannel)
    {
        _dbContext.TextChannels.Remove(textChannel);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TextChannel?> GetByIdAsync(Guid textChannelId) =>
        await _dbContext.TextChannels.FindAsync(textChannelId);

    public IQueryable<TextChannel> GetAsQueryable() => _dbContext.TextChannels.AsQueryable();
}
