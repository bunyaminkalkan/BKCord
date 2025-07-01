namespace BKCordServer.Server.Domain.Entities;

public class RoleMember
{
    public Guid UserId { get; set; }
    public Guid GivenBy { get; set; }
    public Guid ServerId { get; set; }
    public Guid RoleId { get; set; }
}
