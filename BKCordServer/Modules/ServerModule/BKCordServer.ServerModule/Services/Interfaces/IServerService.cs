namespace BKCordServer.ServerModule.Services.Interfaces;
public interface IServerService
{
    Task CreateAsync(Guid userId, string Name, string LogoUrl);
}
