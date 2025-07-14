using BKCordServer.ServerModule.Constants;
using BKCordServer.ServerModule.Contracts;
using BKCordServer.ServerModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.ServerModule.Data.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(Tables.Roles, Tables.ServerSchema);
        builder.HasKey(r => r.Id);

        builder.Property(r => r.ServerId)
            .IsRequired();

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Color)
            .HasMaxLength(7); // Hex color code (#FFFFFF)

        builder.Property(r => r.Hierarchy)
            .IsRequired();

        builder.HasIndex(r => new { r.ServerId, r.Name })
            .IsUnique();

        builder.Property(r => r.RolePermissions)
            .HasConversion(
                v => string.Join(',', v.Select(p => p.ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .Select(s => Enum.Parse<RolePermission>(s.Trim()))
                     .ToList()
            )
            .HasMaxLength(2000);
    }
}
