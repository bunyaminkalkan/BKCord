using BKCordServer.Identity.Data.Context.PostgreSQL;
using BKCordServer.Identity.Domain.Entities;
using BKCordServer.Identity.Repositories;
using BKCordServer.Identity.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Kernel.DependencyInjection;
using Shared.Kernel.Validations;

namespace BKCordServer.Identity;
public class IdentityModule : IModule
{
    private const string SectionName = "PostgreSQL";

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        #region DB
        services.AddDbContext<AppIdentityDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString(SectionName)));

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
            cfg.RegisterServicesFromAssembly(typeof(IdentityModule).Assembly);

            cfg.AddOpenBehavior(typeof(FluentValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(IdentityModule).Assembly);
        #endregion

        #region Scoped
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        #endregion

        Console.WriteLine("Identity module services registered");
    }
}
