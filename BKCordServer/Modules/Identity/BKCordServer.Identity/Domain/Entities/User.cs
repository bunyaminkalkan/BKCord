using BKCordServer.Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace BKCordServer.Identity.Domain.Entities;

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
