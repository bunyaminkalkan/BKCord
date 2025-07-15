using BKCordServer.TextChannelModule.SignalR;
using BKCordServer.WebAPI.Middlewares;
using FluentValidation;
using Shared.Kernel;
using Shared.Kernel.DependencyInjection;
using System.Reflection;
using System.Text.Json.Serialization;

namespace BKCordServer.WebAPI.Extensions;

public static class InstallerExtensions
{
    static readonly List<Assembly> assemblies =
            [
            typeof(IdentityModule.IdentityModule).Assembly,
            typeof(ServerModule.ServerModule).Assembly,
            typeof(TextChannelModule.TextChannelModule).Assembly
            ];

    public static IServiceCollection InstallModules(this IServiceCollection services, IConfiguration configuration)
    {
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

    public static IMvcBuilder AddModularControllers(this IServiceCollection services)
    {
        var mvcBuilder = services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }); ;

        foreach (var assembly in assemblies)
        {
            mvcBuilder.AddApplicationPart(assembly);
        }

        return mvcBuilder;
    }

    public static WebApplication MapHubs(this WebApplication app)
    {
        app.MapHub<ChatHub>("/chatHub");

        return app;
    }
}
