namespace BKCordServer.ServerModule.Domain.Enums;
public enum RolePermission
{
    ManageMessages = 1,

    KickMembers = 100,
    BanMembers = 101,
    ManageRoles = 102,
    AssignRoles = 103,

    CreateChannels = 200,
    ManageChannels = 201,

    ManageServer = 300,

    Administrator = 1000
}
