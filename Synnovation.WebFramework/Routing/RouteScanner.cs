using System.Reflection;
using Synnovation.WebFramework.Core;

namespace Synnovation.WebFramework.Routing;

/// <summary>
/// Scans assemblies for controllers and registers routes based on annotations.
/// </summary>
public static class RouteScanner
{
    public static void RegisterRoutesFromAssembly(RouteTable routeTable, Assembly assembly)
    {
        var controllers = assembly.GetTypes()
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract);

        foreach (var controller in controllers)
        {
            var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(method => method.GetCustomAttributes<HttpVerbAttribute>().Any());

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<HttpVerbAttribute>();
                routeTable.AddRoute(attribute!.Path,
                    attribute.GetType().Name.Replace("Http", "").Replace("Attribute", "").ToUpper(), controller,
                    method.Name);
            }
        }
    }
}