﻿using BKCordServer.ServerModule.Constants;
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

        builder.Property(smh => smh.UserId)
            .IsRequired();

        builder.Property(smh => smh.ServerId)
            .IsRequired();

        builder.Property(smh => smh.JoinedAt)
            .IsRequired();

        builder.Property(smh => smh.LeftAt)
            .IsRequired();

        builder.Property(smh => smh.RemovedReason)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(smh => smh.RemovedByUserId)
            .HasMaxLength(450)
            .IsRequired(false);

        builder.Property(smh => smh.WasBanned)
            .IsRequired();
    }
}
