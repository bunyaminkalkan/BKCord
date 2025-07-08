using System.Text.Json.Serialization;

namespace BKCordServer.WebAPI.Extensions;

public static class ControllerExtensions
{
    public static IMvcBuilder AddModularControllers(this IServiceCollection services)
    {
        var mvcBuilder = services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }); ;

        // Modüllerin Presentation katmanlarındaki AssemblyReference'ları
        var assemblies = new[]
        {
            typeof(IdentityModule.IdentityModule).Assembly,
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
