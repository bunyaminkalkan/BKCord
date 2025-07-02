using BKCordServer.ServerModule.Constants;
using BKCordServer.ServerModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.ServerModule.Data.Configurations;

public sealed class RoleMemberConfiguration : IEntityTypeConfiguration<RoleMember>
{
    public void Configure(EntityTypeBuilder<RoleMember> builder)
    {
        builder.ToTable(Tables.RoleMembers, Tables.ServerSchema);
        builder.HasKey(rm => new { rm.UserId, rm.RoleId, rm.ServerId });

        builder.Property(rm => rm.UserId)
            .IsRequired();

        builder.Property(rm => rm.GivenBy)
            .IsRequired();

        builder.Property(rm => rm.ServerId)
            .IsRequired();

        builder.Property(rm => rm.RoleId)
            .IsRequired();
    }
}
