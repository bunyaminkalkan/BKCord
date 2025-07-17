using BKCordServer.IdentityModule.Contracts;
using BKCordServer.IdentityModule.Data.Context.PostgreSQL;
using BKCordServer.IdentityModule.Domain.Entities;
using BKCordServer.IdentityModule.Options;
using BKCordServer.IdentityModule.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Kernel.BuildingBlocks;
using Shared.Kernel.DependencyInjection;
using Shared.Kernel.Validations;

namespace BKCordServer.IdentityModule;
public class IdentityModule : IModule
{
    private const string SectionName = "PostgreSQL";

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        #region DB
        services.AddDbContext<AppIdentityDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString(SectionName));
            options.AddInterceptors(sp.GetRequiredService<EntityAuditInterceptor>());
        });

        //identity
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<AppIdentityDbContext>();

        services.AddScoped<UserManager<User>>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
        services.AddScoped<IUserValidator<User>, UserValidator<User>>();
        services.AddScoped<IPasswordValidator<User>, PasswordValidator<User>>();
        #endregion

        #region MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(IdentityModule).Assembly,
                typeof(IdentityModuleContractsMarker).Assembly
                );

            cfg.AddOpenBehavior(typeof(FluentValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(IdentityModule).Assembly);
        #endregion

        #region Interfaces
        services.AddScoped<IJwtService, JwtService>();
        #endregion

        #region Auth
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddAuthentication()
            .AddJwtBearer();

        services.AddAuthorization();
        #endregion

        Console.WriteLine("Identity module services registered");
    }
}
