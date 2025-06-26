using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Kernel.DependencyInjection;
using Shared.Kernel.Validations;

namespace BKCordServer.Modules.Identity.Application;

public class IdentityApplicationInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly);

            cfg.AddOpenBehavior(typeof(FluentValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
    }
}