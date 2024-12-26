using System.Reflection;
using Synnovation.WebFramework.Annotations;
using Synnovation.WebFramework.Core;
using Synnovation.WebFramework.Core.Controllers;

namespace Synnovation.WebFramework.Routing;

/// <summary>
/// Scans assemblies for controllers and registers routes based on annotations.
/// </summary>
public static class RouteScanner
{
    public static void RegisterRoutesFromAssembly(RouteTable routeTable, Assembly assembly)
    {
        var controllers = assembly.GetTypes()
            .Where(type => 
                (typeof(MvcControllerBase).IsAssignableFrom(type) 
                 || typeof(ApiControllerBase).IsAssignableFrom(type)) 
                && !type.IsAbstract);

        foreach (var controller in controllers)
        {
            var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (var method in methods)
            {
                var httpVerbAttributes = method.GetCustomAttributes<HttpVerbAttribute>().ToList();
                if (httpVerbAttributes.Count > 0)
                {
                    foreach (var verbAttr in httpVerbAttributes)
                    {
                        var path = verbAttr.Path;
                        var httpMethod = verbAttr.GetType().Name
                            .Replace("Http", "")
                            .Replace("Attribute", "")
                            .ToUpperInvariant();

                        var requiresAuthorization = method.GetCustomAttribute<AuthorizeAttribute>() != null;
                        routeTable.AddRoute(path, httpMethod, controller, method.Name, requiresAuthorization);
                    }
                }
                else
                {
                    // default GET /MethodName
                    var path = $"/{method.Name}";
                    var requiresAuthorization = method.GetCustomAttribute<AuthorizeAttribute>() != null;
                    routeTable.AddRoute(path, "GET", controller, method.Name, requiresAuthorization);
                }
            }
        }
    }
}
