namespace BKCordServer.TextChannelModule.Domain.Entities;
public class TextChannel
{
    public Guid Id { get; set; }
    public Guid ServerId { get; set; }
    public string Name { get; set; }
    public int MessageCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}
