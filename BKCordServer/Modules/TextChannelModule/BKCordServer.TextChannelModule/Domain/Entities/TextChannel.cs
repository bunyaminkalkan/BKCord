using Shared.Kernel.BuildingBlocks;

namespace BKCordServer.TextChannelModule.Domain.Entities;

public class TextChannel : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ServerId { get; set; }
    public string Name { get; set; }
    public int MessageCount { get; set; } = 0;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}
