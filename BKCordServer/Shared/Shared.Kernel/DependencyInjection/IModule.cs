using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Kernel.DependencyInjection;

public interface IModule
{
    void Install(IServiceCollection services, IConfiguration configuration);
}
