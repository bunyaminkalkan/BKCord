using BKCordServer.IdentityModule.Constants;
using BKCordServer.IdentityModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BKCordServer.IdentityModule.Data.Configurations;
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(Tables.Users, Tables.IdentitySchema);

        //IdentityUser zaten tanımlıyor
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name).HasMaxLength(100).IsRequired(false);
        builder.Property(u => u.Middlename).HasMaxLength(100).IsRequired(false);
        builder.Property(u => u.Surname).HasMaxLength(100).IsRequired(false);
        builder.Property(u => u.AvatarUrl).IsRequired(false).HasMaxLength(256);

        builder.Property(u => u.IsPrivateAccount).HasDefaultValue(false);

        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.UpdatedAt).IsRequired(false);
        builder.Property(u => u.IsDeleted).IsRequired();
        builder.Property(u => u.DeletedAt).IsRequired(false);
    }
}
