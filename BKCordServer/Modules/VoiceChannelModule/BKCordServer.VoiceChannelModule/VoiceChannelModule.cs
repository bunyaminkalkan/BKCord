using BKCordServer.VoiceChannelModule.Data.Context.PostgreSQL;
using BKCordServer.VoiceChannelModule.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Kernel.BuildingBlocks;
using Shared.Kernel.DependencyInjection;
using Shared.Kernel.Validations;

namespace BKCordServer.VoiceChannelModule;
public class VoiceChannelModule : IModule
{
    private const string SectionName = "PostgreSQL";

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        #region DB
        services.AddDbContext<AppVoiceChannelDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString(SectionName));
            options.AddInterceptors(sp.GetRequiredService<EntityAuditInterceptor>());
        });

        #endregion

        #region MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(VoiceChannelModule).Assembly
                );

            cfg.AddOpenBehavior(typeof(FluentValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(VoiceChannelModule).Assembly);
        #endregion

        //services.AddHttpClient<IMediasoupService, MediasoupService>();
        services.AddScoped<IMediasoupService, MediasoupService>();

        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
        });

        Console.WriteLine("VoiceChannel module services registered");
    }
}
