using BKCordServer.ServerModule.Constants;
using BKCordServer.ServerModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.ServerModule.Data.Configurations;

public sealed class ServerMembersHistoryConfiguration : IEntityTypeConfiguration<ServerMembersHistory>
{
    public void Configure(EntityTypeBuilder<ServerMembersHistory> builder)
    {
        builder.ToTable(Tables.ServerMembersHistory, Tables.ServerSchema);
        builder.HasKey(smh => smh.Id);

        builder.Property(smh => smh.UserId).IsRequired();
        builder.Property(smh => smh.ServerId).IsRequired();

        builder.Property(smh => smh.RemovedReason)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(smh => smh.RemovedByUserId)
            .IsRequired(false);

        builder.Property(smh => smh.WasBanned).IsRequired();

        builder.Property(smh => smh.CreatedAt).IsRequired();
        builder.Property(smh => smh.UpdatedAt).IsRequired(false);
        builder.Property(smh => smh.IsDeleted).IsRequired();
        builder.Property(smh => smh.DeletedAt).IsRequired(false);
    }
}

