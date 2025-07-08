namespace BKCordServer.IdentityModule.DTOs;

public record JwtResponse(
    string AccessToken,
    string RefreshToken
    );
