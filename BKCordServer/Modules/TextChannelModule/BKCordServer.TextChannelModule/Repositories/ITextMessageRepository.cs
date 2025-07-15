using BKCordServer.TextChannelModule.Domain.Entities;

namespace BKCordServer.TextChannelModule.Repositories;
public interface ITextMessageRepository
{
    Task AddAsync(TextMessage textMessage);
    Task UpdateAsync(TextMessage textMessage);
    IQueryable<TextMessage> GetAsQueryable();
}
