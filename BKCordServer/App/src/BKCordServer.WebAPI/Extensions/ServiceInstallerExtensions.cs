using BKCordServer.WebAPI.Middlewares;
using Shared.Kernel.DependencyInjection;
using System.Reflection;

namespace BKCordServer.WebAPI.Extensions;

public static class ServiceInstallerExtensions
{
    public static IServiceCollection InstallServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = LoadModuleAssemblies();

        var installers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IServiceInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>();

        foreach (var installer in installers)
        {
            installer.Install(services, configuration);
        }

        //webapi
        services.AddScoped<ExceptionMiddleware>();

        return services;
    }

    private static Assembly[] LoadModuleAssemblies()
    {
        var assemblies = new List<Assembly>();
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        var assemblyFiles = Directory.GetFiles(baseDirectory, "*.dll")
            .Where(file => Path.GetFileName(file).StartsWith("BKCordServer.Modules"))
            .ToArray();

        foreach (var assemblyFile in assemblyFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(assemblyFile);
                assemblies.Add(assembly);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not load assembly: {assemblyFile}");
            }
        }

        return assemblies.ToArray();
    }

    static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
        typeof(T).IsAssignableFrom(typeInfo) &&
        !typeInfo.IsInterface &&
        !typeInfo.IsAbstract;
}
