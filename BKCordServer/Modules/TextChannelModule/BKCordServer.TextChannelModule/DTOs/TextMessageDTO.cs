using BKCordServer.IdentityModule.Contracts;

namespace BKCordServer.TextChannelModule.DTOs;
public class TextMessageDTO
{
    public Guid TextMessageId;
    public UserInfDTO User { get; set; }
    public string Content { get; set; }
    public bool IsUpdated { get; set; }

    public TextMessageDTO(Guid textMessageId, UserInfDTO user, string content, bool isUpdated)
    {
        TextMessageId = textMessageId;
        User = user;
        Content = content;
        IsUpdated = isUpdated;
    }
}
