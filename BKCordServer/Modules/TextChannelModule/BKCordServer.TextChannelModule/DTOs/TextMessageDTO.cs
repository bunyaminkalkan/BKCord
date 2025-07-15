using BKCordServer.IdentityModule.Contracts;
using BKCordServer.TextChannelModule.Domain.Entities;

namespace BKCordServer.TextChannelModule.DTOs;
public class TextMessageDTO
{
    public Guid TextMessageId { get; set; }
    public UserInfDTO User { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsUpdated { get; set; }

    public TextMessageDTO(UserInfDTO user, TextMessage textMessage, bool isUpdated)
    {
        TextMessageId = textMessage.Id;
        User = user;
        Content = textMessage.Content;
        CreatedAt = textMessage.CreatedAt;
        IsUpdated = isUpdated;
    }
}
