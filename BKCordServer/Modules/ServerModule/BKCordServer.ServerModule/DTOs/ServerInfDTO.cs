using BKCordServer.ServerModule.Domain.Entities;

namespace BKCordServer.ServerModule.DTOs;
public class ServerInfDTO
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; }
    public string LogoUrl { get; set; }
    public string InviteCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ServerInfDTO(Server server)
    {
        Id = server.Id;
        OwnerId = server.OwnerId;
        Name = server.Name;
        LogoUrl = server.LogoUrl;
        InviteCode = server.InviteCode;
        CreatedAt = server.CreatedAt;
        UpdatedAt = server.UpdatedAt;
    }
}
