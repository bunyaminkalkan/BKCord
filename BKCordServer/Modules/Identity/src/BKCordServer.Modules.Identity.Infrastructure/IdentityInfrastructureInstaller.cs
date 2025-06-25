using BKCordServer.Modules.Identity.Application.Services;
using BKCordServer.Modules.Identity.Domain.Entities;
using BKCordServer.Modules.Identity.Domain.Repositories;
using BKCordServer.Modules.Identity.Infrastructure.Context.PostgreSQL;
using BKCordServer.Modules.Identity.Infrastructure.Persistance.Repositories;
using BKCordServer.Modules.Identity.Infrastructure.Persistance.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Kernel.DependencyInjection;

namespace BKCordServer.Modules.Identity.Infrastructure;
public class IdentityInfrastructureInstaller : IServiceInstaller
{
    private const string SectionName = "PostgreSQL";

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppIdentityDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString(SectionName)));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IUserRepository, UserRepository>();


        //identity
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<AppIdentityDbContext>();

        services.AddScoped<UserManager<User>>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
        services.AddScoped<IUserValidator<User>, UserValidator<User>>();
        services.AddScoped<IPasswordValidator<User>, PasswordValidator<User>>();
    }
}
