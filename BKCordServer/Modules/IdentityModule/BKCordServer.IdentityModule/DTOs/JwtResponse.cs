namespace BKCordServer.IdentityModule.DTOs;

public class JwtResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? ExpirationDate { get; set; }

    // 2FA için ek alanlar
    public bool RequiresTwoFactor { get; set; } = false;
    public string? Message { get; set; }
}
