using BKCordServer.TextChannelModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
public class AppTextChannelDbContext : DbContext
{
    public AppTextChannelDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TextChannel> TextChannels { get; set; }
    public DbSet<TextMessage> TextMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TextChannel>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<TextMessage>().HasQueryFilter(m => !m.IsDeleted);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppTextChannelDbContext).Assembly);
    }
}
