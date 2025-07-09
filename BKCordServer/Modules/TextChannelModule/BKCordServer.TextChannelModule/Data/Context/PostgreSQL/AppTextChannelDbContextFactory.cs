using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
public class AppTextChannelDbContextFactory : IDesignTimeDbContextFactory<AppTextChannelDbContext>
{
    public AppTextChannelDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory());

        // config dosyasını al
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        // connection string'i çek
        var connectionString = configuration.GetConnectionString("PostgreSQL");

        // context options oluştur
        var optionsBuilder = new DbContextOptionsBuilder<AppTextChannelDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppTextChannelDbContext(optionsBuilder.Options);
    }
}
