namespace BKCordServer.IdentityModule.Services;
public interface IPasswordResetTokenService
{
    string GenerateToken();
    string HashToken(string token);
    bool ValidateToken(string token, string hashedToken);
}
