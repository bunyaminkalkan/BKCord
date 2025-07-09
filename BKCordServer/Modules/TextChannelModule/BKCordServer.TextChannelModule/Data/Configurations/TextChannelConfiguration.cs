using BKCordServer.TextChannelModule.Constants;
using BKCordServer.TextChannelModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.TextChannelModule.Data.Configurations;
public class TextChannelConfiguration : IEntityTypeConfiguration<TextChannel>
{

    public void Configure(EntityTypeBuilder<TextChannel> builder)
    {
        builder.ToTable(Tables.TextChannels, Tables.TextChannelSchema);

        builder.HasKey(tc => tc.Id);

        builder.Property(tc => tc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tc => tc.ServerId)
            .IsRequired();

        builder.Property(tc => tc.MessageCount)
            .IsRequired();

        builder.Property(tc => tc.CreatedAt)
            .IsRequired();

        builder.Property(tc => tc.UpdatedAt)
            .IsRequired();

        builder.Property(tc => tc.IsDeleted)
            .IsRequired();
    }
}
