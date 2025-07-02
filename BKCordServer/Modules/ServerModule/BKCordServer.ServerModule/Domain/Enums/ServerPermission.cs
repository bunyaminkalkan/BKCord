namespace BKCordServer.ServerModule.Domain.Enums;
public enum ServerPermission
{
    ManageMessages = 1,

    KickMembers = 10,
    BanMembers = 11,
    ManageRoles = 12,
    AssignRoles = 13,

    CreateChannels = 20,
    ManageChannels = 21,

    ManageServer = 30,

    Administrator = 100
}
