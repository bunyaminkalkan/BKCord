using BKCordServer.IdentityModule.Contracts;

namespace BKCordServer.ServerModule.DTOs;
public class RoleBasedServerUsersDTO
{
    public RoleDTO Role { get; set; }
    public IEnumerable<UserInfDTO> Users { get; set; }
}
