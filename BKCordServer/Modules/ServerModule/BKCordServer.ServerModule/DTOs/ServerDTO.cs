using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.DTOs;
public class ServerDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LogoUrl { get; set; }

    public ServerDTO(Server server)
    {
        Id = server.Id;
        Name = server.Name;
        LogoUrl = server.LogoUrl;
    }
}
