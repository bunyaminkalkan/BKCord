using BKCordServer.Identity;

namespace BKCordServer.WebAPI.Extensions;

public static class ControllerExtensions
{
    public static IMvcBuilder AddModularControllers(this IServiceCollection services)
    {
        var mvcBuilder = services.AddControllers();

        // Modüllerin Presentation katmanlarındaki AssemblyReference'ları
        var assemblies = new[]
        {
            typeof(IdentityModule).Assembly,
            typeof(ServerModule.ServerModule).Assembly,
            // diğer modüller...
        };

        foreach (var assembly in assemblies)
        {
            mvcBuilder.AddApplicationPart(assembly);
        }

        return mvcBuilder;
    }
}
