using BKCordServer.ServerModule.Constants;
using BKCordServer.ServerModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.ServerModule.Data.Configurations;

public sealed class ServerMemberHistoryConfiguration : IEntityTypeConfiguration<ServerMemberHistory>
{
    public void Configure(EntityTypeBuilder<ServerMemberHistory> builder)
    {
        builder.ToTable(Tables.ServerMemberHistory, Tables.ServerSchema);
        builder.HasKey(smh => smh.Id);

        builder.Property(smh => smh.Id)
            .IsRequired();

        builder.Property(smh => smh.UserId)
            .IsRequired();

        builder.Property(smh => smh.ServerId)
            .IsRequired();

        builder.Property(smh => smh.CreatedAt)
            .IsRequired();

        builder.Property(smh => smh.UpdatedAt)
            .IsRequired(false);

        builder.Property(smh => smh.Reason)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(smh => smh.ActionedByUserId)
            .IsRequired(false);

        builder.Property(smh => smh.IsBanned)
            .IsRequired();

        builder.Property(smh => smh.IsKicked)
            .IsRequired();
    }
}