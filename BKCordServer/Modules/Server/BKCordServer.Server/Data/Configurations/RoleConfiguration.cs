using BKCordServer.Server.Constants;
using BKCordServer.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.Server.Data.Configurations;

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
    }
}
