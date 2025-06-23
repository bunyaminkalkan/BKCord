namespace BKCordServer.Modules.Identity.Infrastructure.Context.PostgreSQL;

using BKCordServer.Modules.Identity.Domain.Entities;
using BKCordServer.Modules.Identity.Infrastructure.Configurations;
using BKCordServer.Modules.Identity.Infrastructure.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppIdentityDbContext : IdentityUserContext<User>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }

    //public virtual DbSet<User> Users { get; set; } zaten IdentityDbContext içinde var
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Tablo adlarını ve şemayı özelleştir
        builder.Entity<IdentityUserClaim<string>>(e => { e.ToTable(name: "user_claims", schema: Tables.IdentitySchema); });
        builder.Entity<IdentityUserLogin<string>>(e => { e.ToTable(name: "user_logins", schema: Tables.IdentitySchema); });
        builder.Entity<IdentityUserToken<string>>(e => { e.ToTable(name: "user_tokens", schema: Tables.IdentitySchema); });

        // Konfigürasyonları uygula
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new RefreshTokenConfiguration());
    }
}

