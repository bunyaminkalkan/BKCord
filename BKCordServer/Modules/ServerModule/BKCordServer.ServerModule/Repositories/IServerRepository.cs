namespace BKCordServer.ServerModule.Repositories;
public interface IServerRepository
{
    Task AddAsync(Domain.Entities.Server server);
}
