namespace BKCordServer.WebAPI.Extensions;

public static class CorsExtensions
{
    private const string DefaultPolicyName = "AllowAll";
    private static readonly string[] DefaultAllowedOrigins = {
        "http://localhost:5500",
        "http://127.0.0.1:5500"
    };

    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(DefaultPolicyName, policy =>
            {
                policy.WithOrigins(DefaultAllowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
    {
        app.UseCors(DefaultPolicyName);
        return app;
    }
}

