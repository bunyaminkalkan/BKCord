namespace BKCordServer.ServerModule.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public Guid ServerId { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public short Hierarchy { get; set; }
}
