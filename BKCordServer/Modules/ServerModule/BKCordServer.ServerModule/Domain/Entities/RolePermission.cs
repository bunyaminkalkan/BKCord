using BKCordServer.ServerModule.Domain.Enums;

namespace BKCordServer.ServerModule.Domain.Entities;
public class RolePermission
{
    public Guid RoleId { get; set; }
    public List<ServerPermission> ServerPermissions { get; set; } = new List<ServerPermission>();
}
