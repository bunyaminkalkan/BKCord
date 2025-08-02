using Shared.Kernel.BuildingBlocks;

namespace BKCordServer.ServerModule.Domain.Entities;
public class Server : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OwnerId { get; set; }
    public string Name { get; set; }
    public string LogoUrl { get; set; }
    public string InviteCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
