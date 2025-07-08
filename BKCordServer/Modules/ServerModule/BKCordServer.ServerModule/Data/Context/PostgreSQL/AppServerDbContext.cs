using BKCordServer.ServerModule.Domain.Entities;
using BKCordServer.ServerModule.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.ServerModule.Data.Context.PostgreSQL;

public class AppServerDbContext : DbContext
{
    public AppServerDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Server> Servers { get; set; }
    public DbSet<ServerMember> ServerMembers { get; set; }
    public DbSet<ServerMembersHistory> ServerMembersHistory { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleMember> RoleMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Global Query Filter - Status'u Deleted olmayanları getir
        modelBuilder.Entity<Server>()
            .HasQueryFilter(s => s.Status != ServerStatus.Deleted);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppServerDbContext).Assembly);
    }
}
