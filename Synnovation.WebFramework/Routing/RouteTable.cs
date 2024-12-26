namespace Synnovation.WebFramework.Routing;

/// <summary>
/// Singleton storage and management for all registered routes.
/// </summary>
public class RouteTable
{
    private static readonly Lazy<RouteTable> _instance = new(() => new RouteTable());
    public static RouteTable Instance => _instance.Value;

    public List<RouteConfig> Routes { get; } = [];

    private RouteTable() { }

    public void AddRoute(string path, string httpMethod, Type controllerType, string actionName, bool requiresAuth)
    {
        var config = new RouteConfig(path, httpMethod, controllerType, actionName, requiresAuth);
        Routes.Add(config);
    }
}