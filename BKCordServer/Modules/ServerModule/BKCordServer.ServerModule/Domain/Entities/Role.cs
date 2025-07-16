using BKCordServer.ServerModule.Contracts;
using Shared.Kernel.BuildingBlocks;

namespace BKCordServer.ServerModule.Domain.Entities;

public class Role : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ServerId { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public short Hierarchy { get; set; }
    public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
