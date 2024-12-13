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
                // Check if the method has an HttpVerbAttribute (e.g., [HttpGet])
                var httpVerbAttribute = method.GetCustomAttribute<HttpVerbAttribute>();

                string path;
                string httpMethod;

                if (httpVerbAttribute != null)
                {
                    // If an HttpVerbAttribute is present, use its values
                    path = httpVerbAttribute.Path ?? $"/{method.Name}";
                    httpMethod = httpVerbAttribute.GetType().Name.Replace("Http", "").Replace("Attribute", "")
                        .ToUpper();
                }
                else
                {
                    // If no HttpVerbAttribute, default to the action name as route and assume GET method
                    path = $"/{method.Name}";
                    httpMethod = "GET";
                }

                // Determine if the method requires authorization
                var requiresAuthorization = method.GetCustomAttribute<AuthorizeAttribute>() != null;

                // Add the route to the RouteTable
                routeTable.AddRoute(path, httpMethod, controller, method.Name, requiresAuthorization);
            }
        }
    }
}