namespace BKCordServer.IdentityModule.Options;
public sealed class MailSettingOptions
{
    public string Email { get; set; }
    public string Smtp { get; set; }
    public int Port { get; set; }
    public string SSL { get; set; }
    public string UserId { get; set; }
    public string Password { get; set; }
}
