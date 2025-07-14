using BKCordServer.TextChannelModule.Domain.Entities;

namespace BKCordServer.TextChannelModule.Repositories;
public interface ITextChannelRepository
{
    Task AddAsync(TextChannel textChannel);
    Task UpdateAsync(TextChannel textChannel);
    Task RemoveAsync(TextChannel textChannel);
    Task<TextChannel?> GetByIdAsync(Guid textChannelId);
    IQueryable<TextChannel> GetAsQueryable();
}
