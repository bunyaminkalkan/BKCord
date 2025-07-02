using BKCordServer.ServerModule.Domain.Enums;

namespace BKCordServer.ServerModule.Domain.Entities;
public class Server
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; }
    public string LogoUrl { get; set; }
    public string InviteCode { get; set; }
    public ServerStatus Status { get; set; } = ServerStatus.Active;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
