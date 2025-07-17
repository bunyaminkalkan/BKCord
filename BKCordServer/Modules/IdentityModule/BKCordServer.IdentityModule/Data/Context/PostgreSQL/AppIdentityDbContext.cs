namespace BKCordServer.IdentityModule.Data.Context.PostgreSQL;

using BKCordServer.IdentityModule.Constants;
using BKCordServer.IdentityModule.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppIdentityDbContext : IdentityUserContext<User, Guid>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }

    //public virtual DbSet<User> Users { get; set; } zaten IdentityDbContext içinde var
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Tablo adlarını ve şemayı özelleştir
        builder.Entity<IdentityUserClaim<Guid>>(e => { e.ToTable(name: "user_claims", schema: Tables.IdentitySchema); });
        builder.Entity<IdentityUserLogin<Guid>>(e => { e.ToTable(name: "user_logins", schema: Tables.IdentitySchema); });
        builder.Entity<IdentityUserToken<Guid>>(e => { e.ToTable(name: "user_tokens", schema: Tables.IdentitySchema); });

        builder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        builder.Entity<RefreshToken>().HasQueryFilter(r => !r.IsDeleted);

        // Konfigürasyonları uygula
        builder.ApplyConfigurationsFromAssembly(typeof(AppIdentityDbContext).Assembly);
    }
}

