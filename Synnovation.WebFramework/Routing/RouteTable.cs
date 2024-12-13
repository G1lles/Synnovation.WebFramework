namespace Synnovation.WebFramework.Routing;

/// <summary>
/// Singleton storage and management for all registered routes.
/// </summary>
public class RouteTable
{
    private static readonly Lazy<RouteTable> _instance = new(() => new RouteTable());
    public static RouteTable Instance => _instance.Value;

    /// <summary>
    /// List of all registered routes.
    /// </summary>
    public List<RouteConfig> Routes { get; } = [];

    private RouteTable() { }

    /// <summary>
    /// Adds a new route to the route table.
    /// </summary>
    /// <param name="path">The route path (e.g., /user/{id}).</param>
    /// <param name="httpMethod">The HTTP method (GET, POST, etc.).</param>
    /// <param name="controllerType">The controller type to invoke.</param>
    /// <param name="actionName">The action method to invoke.</param>
    public void AddRoute(string path, string httpMethod, Type controllerType, string actionName)
    {
        Routes.Add(new RouteConfig(path, httpMethod, controllerType, actionName));
    }
}