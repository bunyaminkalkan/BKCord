using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Kernel.BuildingBlocks;
using Shared.Kernel.Images;
using Shared.Kernel.Options;
using Shared.Kernel.Services;

namespace Shared.Kernel;
public static class SharedInstaller
{
    public static void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.ConfigureOptions<ImageOptionsSetup>();
        services.AddScoped<EntityAuditInterceptor>();

        #region Interfaces
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IHttpContextService, HttpContextService>();
        #endregion

        Console.WriteLine("Shared services registered");
    }
}
