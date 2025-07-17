using Microsoft.AspNetCore.Identity;
using Shared.Kernel.BuildingBlocks;

namespace BKCordServer.IdentityModule.Domain.Entities;

public sealed class User : IdentityUser<Guid>, IEntity
{
    public string Name { get; set; }
    public string Middlename { get; set; }
    public string Surname { get; set; }
    public string? AvatarUrl { get; set; }
    public bool IsPrivateAccount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}
