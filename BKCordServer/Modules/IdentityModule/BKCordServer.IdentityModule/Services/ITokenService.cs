namespace BKCordServer.IdentityModule.Services;
public interface ITokenService
{
    string GenerateToken();
    string HashToken(string token);
    bool ValidateToken(string token, string hashedToken);
}
