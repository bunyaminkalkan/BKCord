namespace BKCordServer.ServerModule.Domain.Enums;
public enum RolePermission
{
    ManageMessages = 1,

    KickMembers = 100,
    BanMembers = 101,

    ManageRoles = 200,

    CreateChannels = 300,
    ManageChannels = 301,

    ManageServer = 400,

    Administrator = 1000
}
