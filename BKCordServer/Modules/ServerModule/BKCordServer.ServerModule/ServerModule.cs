using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Kernel.DependencyInjection;
using Shared.Kernel.Validations;

namespace BKCordServer.ServerModule;
public class ServerModule : IModule
{
    private const string SectionName = "PostgreSQL";

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        #region DB
        services.AddDbContext<AppServerDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString(SectionName)));
        #endregion

        #region MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServerModule).Assembly);

            cfg.AddOpenBehavior(typeof(FluentValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ServerModule).Assembly);
        #endregion

        Console.WriteLine("Server module services registered");
    }
}
