using BKCordServer.Server.Domain.Enums;

namespace BKCordServer.Server.Domain.Entities;
public class RolePermission
{
    public Guid RoleId { get; set; }
    public List<ServerPermission> ServerPermissions { get; set; } = new List<ServerPermission>();
}
