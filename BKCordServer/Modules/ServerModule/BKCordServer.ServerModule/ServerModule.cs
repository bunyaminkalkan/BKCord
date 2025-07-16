using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Contracts;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Kernel.BuildingBlocks;
using Shared.Kernel.DependencyInjection;
using Shared.Kernel.Validations;

namespace BKCordServer.ServerModule;
public class ServerModule : IModule
{
    private const string SectionName = "PostgreSQL";

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        #region DB
        services.AddDbContext<AppServerDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString(SectionName));
            options.AddInterceptors(sp.GetRequiredService<EntityAuditInterceptor>());
        });
        #endregion

        #region MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(ServerModule).Assembly,
                typeof(ServerModuleContractsMarker).Assembly
                );

            cfg.AddOpenBehavior(typeof(FluentValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ServerModule).Assembly);
        #endregion

        #region Interfaces
        services.AddScoped<IServerAuthorizationService, ServerAuthorizationService>();
        #endregion

        Console.WriteLine("Server module services registered");
    }
}
