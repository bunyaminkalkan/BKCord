namespace BKCordServer.IdentityModule.Contracts;
public sealed record UserInfDTO(
    Guid Id,
    string UserName,
    string AvatarUrl
    );
