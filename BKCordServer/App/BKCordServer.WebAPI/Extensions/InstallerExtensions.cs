using BKCordServer.TextChannelModule.SignalR;
using BKCordServer.VoiceChannelModule.SignalR;
using BKCordServer.WebAPI.Middlewares;
using FluentValidation;
using Shared.Kernel;
using Shared.Kernel.DependencyInjection;
using Shared.Kernel.Exceptions;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

namespace BKCordServer.WebAPI.Extensions;

public static class InstallerExtensions
{
    static readonly List<Assembly> assemblies =
            [
            typeof(IdentityModule.IdentityModule).Assembly,
            typeof(ServerModule.ServerModule).Assembly,
            typeof(TextChannelModule.TextChannelModule).Assembly,
            typeof(VoiceChannelModule.VoiceChannelModule).Assembly
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
        app.MapHub<VoiceHub>("/voiceHub");

        return app;
    }

    public static IServiceCollection InstallRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            RateLimitPartition.GetFixedWindowLimiter("global", _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                QueueLimit = 100,
                Window = TimeSpan.FromSeconds(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));

            options.AddPolicy("login", httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: key => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                QueueLimit = 2,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));

            options.AddPolicy("forgot-password", httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: key => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 2,
                QueueLimit = 1,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));

            options.AddPolicy("reset-password", httpContext =>
            RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: key => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                QueueLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.ContentType = "application/json";

                var response = new ErrorResult
                {
                    Message = "Too many requests",
                    StatusCode = StatusCodes.Status429TooManyRequests
                };

                await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken: token);
            };
        });

        return services;
    }
}
