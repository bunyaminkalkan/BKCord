namespace BKCordServer.Modules.Identity.Domain.Entities;
public sealed class RefreshToken
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}