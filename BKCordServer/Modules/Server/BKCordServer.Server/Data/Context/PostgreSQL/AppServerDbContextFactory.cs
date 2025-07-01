using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BKCordServer.Server.Data.Context.PostgreSQL;
internal class AppServerDbContextFactory : IDesignTimeDbContextFactory<AppServerDbContext>
{
    public AppServerDbContext CreateDbContext(string[] args)
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
        var optionsBuilder = new DbContextOptionsBuilder<AppServerDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppServerDbContext(optionsBuilder.Options);
    }
}
