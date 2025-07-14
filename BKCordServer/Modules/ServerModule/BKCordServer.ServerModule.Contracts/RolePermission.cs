namespace BKCordServer.ServerModule.Contracts;
public enum RolePermission
{
    ManageMessages = 1,

    KickMembers = 100,
    BanMembers = 101,

    ManageRoles = 200,

    ManageChannels = 300,

    ManageServer = 400,

    Administrator = 1000
}
