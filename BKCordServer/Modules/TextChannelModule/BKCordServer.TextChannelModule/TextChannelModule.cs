using BKCordServer.TextChannelModule.Data.Context.PostgreSQL;
using BKCordServer.TextChannelModule.Repositories;
using BKCordServer.TextChannelModule.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Kernel.DependencyInjection;
using Shared.Kernel.Validations;

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

        #region Interfaces
        services.AddScoped<ITextChannelRepository, TextChannelRepository>();

        services.AddScoped<ITextChannelService, TextChannelService>();
        #endregion

        Console.WriteLine("TextChannel module services registered");
    }
}
