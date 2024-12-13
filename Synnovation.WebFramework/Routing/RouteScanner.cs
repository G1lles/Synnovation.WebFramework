using System.Reflection;
using Synnovation.WebFramework.Annotations;
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
            var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public);

            foreach (var method in methods)
            {
                var httpVerbAttributes = method.GetCustomAttributes<HttpVerbAttribute>().ToList();

                if (httpVerbAttributes.Count != 0)
                {
                    foreach (var httpVerbAttribute in httpVerbAttributes)
                    {
                        var path = httpVerbAttribute.Path;

                        var httpMethod = httpVerbAttribute.GetType().Name
                            .Replace("Http", "").Replace("Attribute", "").ToUpper();

                        var requiresAuthorization = method.GetCustomAttribute<AuthorizeAttribute>() != null;

                        routeTable.AddRoute(path, httpMethod, controller, method.Name, requiresAuthorization);
                    }
                }
                else
                {
                    var path = $"/{method.Name}";
                    const string httpMethod = "GET";
                    var requiresAuthorization = method.GetCustomAttribute<AuthorizeAttribute>() != null;

                    routeTable.AddRoute(path, httpMethod, controller, method.Name, requiresAuthorization);
                }
            }
        }
    }
}