using BKCordServer.VoiceChannelModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKCordServer.VoiceChannelModule.Data.Context.PostgreSQL;
public class AppVoiceChannelDbContext : DbContext
{
    public AppVoiceChannelDbContext(DbContextOptions<AppVoiceChannelDbContext> options) : base(options)
    {
    }

    public DbSet<VoiceChannel> VoiceChannels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VoiceChannel>().HasQueryFilter(vc => !vc.IsDeleted);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppVoiceChannelDbContext).Assembly);
    }
}
