using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Kernel.DependencyInjection;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}
