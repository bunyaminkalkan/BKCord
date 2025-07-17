using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BKCordServer.VoiceChannelModule.Data.Context.PostgreSQL;
public class AppVoiceChannelDbContextFactory : IDesignTimeDbContextFactory<AppVoiceChannelDbContext>
{
    public AppVoiceChannelDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory());

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("PostgreSQL");

        var optionsBuilder = new DbContextOptionsBuilder<AppVoiceChannelDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppVoiceChannelDbContext(optionsBuilder.Options);
    }
}
