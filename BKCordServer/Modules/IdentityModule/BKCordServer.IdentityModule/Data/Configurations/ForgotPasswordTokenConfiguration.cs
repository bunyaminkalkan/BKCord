using BKCordServer.IdentityModule.Constants;
using BKCordServer.IdentityModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.IdentityModule.Data.Configurations;
public sealed class ForgotPasswordTokenConfiguration : IEntityTypeConfiguration<ForgotPasswordToken>
{
    public void Configure(EntityTypeBuilder<ForgotPasswordToken> builder)
    {
        builder.ToTable(Tables.ForgotPasswordToken, Tables.IdentitySchema);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.Property(x => x.IsUsed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.UsedAt)
            .IsRequired(false);

        // Foreign Key
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_ForgotPasswordTokens_UserId");

        builder.HasIndex(x => x.ExpiresAt)
            .HasDatabaseName("IX_ForgotPasswordTokens_ExpiresAt");

        builder.HasIndex(x => new { x.UserId, x.IsUsed, x.ExpiresAt })
            .HasDatabaseName("IX_ForgotPasswordTokens_UserId_IsUsed_ExpiresAt");
    }
}
