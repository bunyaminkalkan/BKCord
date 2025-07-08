using BKCordServer.WebAPI.Middlewares;
using FluentValidation;
using Shared.Kernel;
using Shared.Kernel.DependencyInjection;
using System.Reflection;

namespace BKCordServer.WebAPI.Extensions;

public static class InstallerExtensions
{
    public static IServiceCollection InstallModules(this IServiceCollection services, IConfiguration configuration)
    {
        List<Assembly> assemblies =
            [
            typeof(IdentityModule.IdentityModule).Assembly,
            typeof(ServerModule.ServerModule).Assembly
            ];

        var installers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IModule>)
            .Select(Activator.CreateInstance)
            .Cast<IModule>();

        foreach (var installer in installers)
        {
            installer.Install(services, configuration);
        }

        //webapi
        services.AddScoped<ExceptionMiddleware>();

        return services;
    }

    static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
        typeof(T).IsAssignableFrom(typeInfo) &&
        !typeInfo.IsInterface &&
        !typeInfo.IsAbstract;

    public static IServiceCollection InstallSharedServices(this IServiceCollection services, IConfiguration configuration)
    {
        SharedInstaller.Install(services, configuration);
        return services;
    }
}
