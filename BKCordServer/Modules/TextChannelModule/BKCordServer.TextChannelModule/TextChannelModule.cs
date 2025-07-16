using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Kernel.BuildingBlocks;
using Shared.Kernel.DependencyInjection;
using Shared.Kernel.Validations;

namespace BKCordServer.TextChannelModule;
public class TextChannelModule : IModule
{
    private const string SectionName = "PostgreSQL";

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        #region DB
        services.AddDbContext<AppTextChannelDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString(SectionName));
            options.AddInterceptors(sp.GetRequiredService<EntityAuditInterceptor>());
        });

        #endregion

        #region MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(TextChannelModule).Assembly
                );

            cfg.AddOpenBehavior(typeof(FluentValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(TextChannelModule).Assembly);
        #endregion

        services.AddSignalR();

        Console.WriteLine("TextChannel module services registered");
    }
}
