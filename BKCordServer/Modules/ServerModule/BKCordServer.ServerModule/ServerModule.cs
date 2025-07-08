using BKCordServer.ServerModule.Commons.Helpers;
using BKCordServer.ServerModule.Data.Context.PostgreSQL;
using BKCordServer.ServerModule.Repositories.Classes;
using BKCordServer.ServerModule.Repositories.Interfaces;
using BKCordServer.ServerModule.Services.Classes;
using BKCordServer.ServerModule.Services.Interfaces;
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

        #region Interfaces
        services.AddScoped<IServerRepository, ServerRepository>();
        services.AddScoped<IServerMemberRepository, ServerMemberRepository>();
        services.AddScoped<IServerMembersHistoryRepository, ServerMembersHistoryRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleMemberRepository, RoleMemberRepository>();

        services.AddScoped<IServerService, ServerService>();
        services.AddScoped<IServerMemberService, ServerMemberService>();
        services.AddScoped<IServerMembersHistoryService, ServerMembersHistoryService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IRoleMemberService, RoleMemberService>();

        services.AddScoped<IPermissionHelperService, PermissionHelperService>();
        #endregion

        Console.WriteLine("Server module services registered");
    }
}
