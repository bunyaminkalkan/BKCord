using BKCordServer.Identity.Constants;
using BKCordServer.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.Identity.Data.Configurations;
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(Tables.Users, Tables.IdentitySchema);

        //IdentityUser zaten tanımlıyor
        //builder.HasKey(u => u.Id);

        builder.Property(u => u.Name).HasMaxLength(100).IsRequired(false);
        builder.Property(u => u.Middlename).HasMaxLength(100).IsRequired(false);
        builder.Property(u => u.Surname).HasMaxLength(100).IsRequired(false);
        builder.Property(u => u.AvatarUrl).HasMaxLength(250);

        builder.Property(u => u.IsPrivateAccount).HasDefaultValue(false);

        builder.Property(u => u.Status).HasConversion<short>();

        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.UpdatedAt).IsRequired();
        builder.Property(u => u.DeletedAt).IsRequired();
    }
}
