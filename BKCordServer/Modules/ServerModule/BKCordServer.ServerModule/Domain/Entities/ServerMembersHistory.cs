using Shared.Kernel.BuildingBlocks;

namespace BKCordServer.ServerModule.Domain.Entities;

public class ServerMembersHistory : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid ServerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? RemovedReason { get; set; }
    public Guid? RemovedByUserId { get; set; }
    public bool WasBanned { get; set; } = false;
}
