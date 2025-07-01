using BKCordServer.Server.Constants;
using BKCordServer.Server.Domain.Entities;
using BKCordServer.Server.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.Server.Data.Configurations;

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable(Tables.RolePermissions, Tables.ServerSchema);
        builder.HasKey(rp => rp.RoleId);

        builder.Property(rp => rp.RoleId)
            .IsRequired();

        builder.Property(rp => rp.ServerPermissions)
            .HasConversion(
                v => string.Join(',', v.Select(p => (int)p)),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .Select(s => (ServerPermission)int.Parse(s))
                     .ToList()
            )
            .HasMaxLength(1000);
    }
}
