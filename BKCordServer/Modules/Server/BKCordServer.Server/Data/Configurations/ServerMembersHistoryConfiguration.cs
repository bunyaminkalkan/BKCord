using BKCordServer.Server.Constants;
using BKCordServer.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.Server.Data.Configurations;

public sealed class ServerMembersHistoryConfiguration : IEntityTypeConfiguration<ServerMembersHistory>
{
    public void Configure(EntityTypeBuilder<ServerMembersHistory> builder)
    {
        builder.ToTable(Tables.ServerMembersHistory, Tables.ServerSchema);
        builder.HasKey(smh => smh.Id);

        builder.Property(smh => smh.UserId)
            .IsRequired();

        builder.Property(smh => smh.ServerId)
            .IsRequired();

        builder.Property(smh => smh.JoinedAt)
            .IsRequired();

        builder.Property(smh => smh.LeftAt)
            .IsRequired();

        builder.Property(smh => smh.RemovedReason)
            .HasMaxLength(500);

        builder.Property(smh => smh.RemovedByUserId)
            .HasMaxLength(450);

        builder.Property(smh => smh.WasBanned)
            .IsRequired();
    }
}
