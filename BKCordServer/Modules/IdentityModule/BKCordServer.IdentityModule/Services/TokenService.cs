using System.Security.Cryptography;
using System.Text;

namespace BKCordServer.IdentityModule.Services;
public class TokenService : ITokenService
{
    public string GenerateToken()
    {
        var tokenBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(tokenBytes);
        }
        return Convert.ToBase64String(tokenBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }

    public string HashToken(string token)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public bool ValidateToken(string token, string hashedToken)
    {
        var computedHash = HashToken(token);
        return computedHash == hashedToken;
    }
}
