using BKCordServer.TextChannelModule.Constants;
using BKCordServer.TextChannelModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.TextChannelModule.Data.Configurations;
public class TextMessageConfiguration : IEntityTypeConfiguration<TextMessage>
{
    public void Configure(EntityTypeBuilder<TextMessage> builder)
    {
        builder.ToTable(Tables.TextMessages, Tables.TextChannelSchema);

        builder.HasKey(tm => tm.Id);

        builder.Property(tm => tm.ChannelId)
            .IsRequired();

        builder.Property(tm => tm.SenderUserId)
            .IsRequired();

        builder.Property(tm => tm.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(tm => tm.CreatedAt)
            .IsRequired();

        builder.Property(tm => tm.UpdatedAt)
            .IsRequired(false);

        builder.Property(tm => tm.IsDeleted)
            .IsRequired();

        builder.HasOne<TextChannel>()
               .WithMany()
               .HasForeignKey(tm => tm.ChannelId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
