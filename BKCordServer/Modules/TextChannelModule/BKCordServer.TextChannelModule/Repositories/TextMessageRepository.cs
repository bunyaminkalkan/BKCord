using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using BKCordServer.TextChannelModule.Domain.Entities;

namespace BKCordServer.TextChannelModule.Repositories;
public class TextMessageRepository : ITextMessageRepository
{
    private readonly AppTextChannelDbContext _dbContext;

    public TextMessageRepository(AppTextChannelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TextMessage textMessage)
    {
        await _dbContext.TextMessages.AddAsync(textMessage);
        await _dbContext.SaveChangesAsync();
    }

    public IQueryable<TextMessage> GetAsQueryable() => _dbContext.TextMessages.AsQueryable();
}
