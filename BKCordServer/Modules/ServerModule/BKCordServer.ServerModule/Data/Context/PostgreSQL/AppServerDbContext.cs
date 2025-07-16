using BKCordServer.ServerModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.ServerModule.Data.Context.PostgreSQL;

public class AppServerDbContext : DbContext
{
    public AppServerDbContext(DbContextOptions<AppServerDbContext> options) : base(options)
    {
    }

    public DbSet<Server> Servers { get; set; }
    public DbSet<ServerMember> ServerMembers { get; set; }
    public DbSet<ServerMembersHistory> ServerMembersHistory { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleMember> RoleMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Server>().HasQueryFilter(s => !s.IsDeleted);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppServerDbContext).Assembly);
    }
}
