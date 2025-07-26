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
using Microsoft.Extensions.Options;
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
        services.AddIdentityCore<User>(options =>
        {
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
        })
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<UserManager<User>>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
        services.AddScoped<IUserValidator<User>, UserValidator<User>>();
        services.AddScoped<IPasswordValidator<User>, PasswordValidator<User>>();

        services.AddScoped<ITwoFactorAuthService, TwoFactorAuthService>();

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(5);
        });
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
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<ITokenService, TokenService>();
        #endregion

        #region Auth
        services.AddHttpContextAccessor();

        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.Configure<MailSettingOptions>(configuration.GetSection("MailSettings"));

        services.AddAuthentication()
            .AddJwtBearer();

        services.AddAuthorization();

        using var scope = services.BuildServiceProvider().CreateScope();
        var mailSettings = scope.ServiceProvider.GetRequiredService<IOptions<MailSettingOptions>>();

        if (string.IsNullOrEmpty(mailSettings.Value.UserId))
            services.AddFluentEmail(mailSettings.Value.Email).AddSmtpSender(mailSettings.Value.Smtp, mailSettings.Value.Port);

        else
            services.AddFluentEmail(mailSettings.Value.Email).AddSmtpSender(mailSettings.Value.Smtp, mailSettings.Value.Port, mailSettings.Value.UserId, mailSettings.Value.Password);
        #endregion

        Console.WriteLine("Identity module services registered");
    }
}
