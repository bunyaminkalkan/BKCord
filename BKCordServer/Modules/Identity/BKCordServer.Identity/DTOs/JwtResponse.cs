namespace BKCordServer.Identity.DTOs;

public record JwtResponse(
    string AccessToken,
    string RefreshToken
    );
