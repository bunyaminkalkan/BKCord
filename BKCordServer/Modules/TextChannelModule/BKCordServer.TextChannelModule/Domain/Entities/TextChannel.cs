namespace BKCordServer.TextChannelModule.Domain.Entities;
public class TextChannel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ServerId { get; set; }
    public Guid CreatedBy { get; set; }
    public string Name { get; set; }
    public int MessageCount { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}
