using BKCordServer.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.Server.Data.Context.PostgreSQL;

public class AppServerDbContext : DbContext
{
    public AppServerDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Server> Servers { get; set; }
    public DbSet<ServerMember> ServerMembers { get; set; }
    public DbSet<ServerMembersHistory> ServerMembersHistory { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<RoleMember> RoleMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppServerDbContext).Assembly);
}
