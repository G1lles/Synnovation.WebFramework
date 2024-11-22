using Synnovation.WebFramework.Core;
using Synnovation.WebFramework.Core.Middleware;
using Synnovation.WebFramework.Routing;
using System.Reflection;

namespace Synnovation.WebFramework.Builder;

/// <summary>
/// Builder for configuring and initializing the web application.
/// </summary>
public class WebAppBuilder
{
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly MiddlewarePipeline _middleware = new();
    private readonly string[] _prefixes;

    public WebAppBuilder(string[] prefixes)
    {
        if (prefixes == null || prefixes.Length == 0)
            throw new ArgumentException("You must specify at least one prefix for the application.");
        _prefixes = prefixes;
    }

    public WebAppBuilder ConfigureRoutes(Action<RouteTable> configureRoutes)
    {
        configureRoutes(RouteTable.Instance);
        return this;
    }

    public WebAppBuilder AutoRegisterControllers(Assembly assembly)
    {
        RouteScanner.RegisterRoutesFromAssembly(RouteTable.Instance, assembly);
        return this;
    }

    public WebAppBuilder ConfigureMiddleware(Action<MiddlewarePipeline> configureMiddleware)
    {
        configureMiddleware(_middleware);
        return this;
    }

    public WebAppBuilder ConfigureServices(Action<IServiceCollection> configureServices)
    {
        configureServices(_services);
        return this;
    }

    public void Run()
    {
        var listener = new HttpListenerService(_prefixes, _middleware);
        listener.RunAsync().GetAwaiter().GetResult();
    }
}