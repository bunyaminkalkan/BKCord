using BKCordServer.VoiceChannelModule.Constants;
using BKCordServer.VoiceChannelModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.VoiceChannelModule.Data.Configurations;
public class VoiceChannelConfiguration : IEntityTypeConfiguration<VoiceChannel>
{
    public void Configure(EntityTypeBuilder<VoiceChannel> builder)
    {
        builder.ToTable(Tables.VoiceChannels, Tables.VoiceChannelSchema);

        builder.HasKey(vc => vc.Id);

        builder.Property(vc => vc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(vc => vc.ServerId)
            .IsRequired();

        builder.Property(vc => vc.CreatedBy)
            .IsRequired();

        builder.Property(vc => vc.CreatedAt)
            .IsRequired();

        builder.Property(vc => vc.UpdatedBy)
            .IsRequired(false);

        builder.Property(vc => vc.UpdatedAt)
            .IsRequired(false);

        builder.Property(vc => vc.IsDeleted)
            .IsRequired();

        builder.Property(vc => vc.DeletedBy)
            .IsRequired(false);

        builder.Property(vc => vc.DeletedAt)
            .IsRequired(false);
    }
}
