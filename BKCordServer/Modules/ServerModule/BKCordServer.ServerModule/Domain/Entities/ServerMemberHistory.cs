namespace BKCordServer.ServerModule.Domain.Entities;

public class ServerMemberHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid ServerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Reason { get; set; }
    public Guid? ActionedByUserId { get; set; }
    public bool IsBanned { get; set; } = false;
    public bool IsKicked { get; set; } = false;
}
