using BKCordServer.IdentityModule.Contracts;

namespace BKCordServer.TextChannelModule.DTOs;
public class TextMessageDTO
{
    public UserInfDTO User { get; set; }
    public string Content { get; set; }

    public TextMessageDTO(UserInfDTO user, string content)
    {
        User = user;
        Content = content;
    }
}
