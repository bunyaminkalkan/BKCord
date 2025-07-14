using BKCordServer.ServerModule.Contracts;

namespace BKCordServer.ServerModule.Domain.Entities;

public class Role
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ServerId { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public short Hierarchy { get; set; }
    public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
