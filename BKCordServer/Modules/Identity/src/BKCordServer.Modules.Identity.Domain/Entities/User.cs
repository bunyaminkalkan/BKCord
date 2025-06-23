using BKCordServer.Modules.Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace BKCordServer.Modules.Identity.Domain.Entities;

public sealed class User : IdentityUser
{
    public string Name { get; set; }
    public string Middlename { get; set; }
    public string Surname { get; set; }
    public string AvatarUrl { get; set; }
    public bool IsPrivateAccount { get; set; }
    public UserStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}
