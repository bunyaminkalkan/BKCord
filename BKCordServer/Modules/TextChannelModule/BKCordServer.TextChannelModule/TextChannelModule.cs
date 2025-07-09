using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Kernel.DependencyInjection;

namespace BKCordServer.TextChannelModule;
public class TextChannelModule : IModule
{
    private const string SectionName = "PostgreSQL";

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        #region DB
        services.AddDbContext<AppTextChannelDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString(SectionName)));
        #endregion

        Console.WriteLine("TextChannel module services registered");
    }
}
