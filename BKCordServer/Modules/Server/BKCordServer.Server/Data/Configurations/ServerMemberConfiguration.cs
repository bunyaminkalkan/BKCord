using BKCordServer.Server.Constants;
using BKCordServer.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.Server.Data.Configurations;

public sealed class ServerMemberConfiguration : IEntityTypeConfiguration<ServerMember>
{
    public void Configure(EntityTypeBuilder<ServerMember> builder)
    {
        builder.ToTable(Tables.ServerMembers, Tables.ServerSchema);
        builder.HasKey(sm => new { sm.UserId, sm.ServerId });

        builder.Property(sm => sm.UserId)
            .IsRequired();

        builder.Property(sm => sm.ServerId)
            .IsRequired();
    }
}
