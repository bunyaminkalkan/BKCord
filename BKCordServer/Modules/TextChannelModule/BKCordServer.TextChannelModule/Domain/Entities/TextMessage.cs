namespace BKCordServer.TextChannelModule.Domain.Entities;

public class TextMessage
{
    public Guid Id { get; set; }
    public Guid ChannelId { get; set; }
    public Guid SenderUserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
