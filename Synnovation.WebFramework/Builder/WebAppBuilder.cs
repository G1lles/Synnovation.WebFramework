using Synnovation.WebFramework.Core;
using Synnovation.WebFramework.Core.Middleware;
using Synnovation.WebFramework.Routing;
using System.Reflection;
using Synnovation.WebFramework.Core.Middleware.Implementations;

namespace Synnovation.WebFramework.Builder;

/// <summary>
/// Builder for configuring and initializing the web application.
/// </summary>
public class WebAppBuilder
{
    private readonly MiddlewarePipeline _middleware = new();
    private readonly string _launchUrl;

    public WebAppBuilder(string launchUrl)
    {
        if (string.IsNullOrEmpty(launchUrl))
            throw new ArgumentException("You must specify at least one prefix for the application.");
        _launchUrl = launchUrl;
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

    public WebAppBuilder UseStaticFiles()
    {
        _middleware.Use(new StaticFileMiddleware());
        return this;
    }

    public WebAppBuilder UseLoggingMiddleware()
    {
        _middleware.Use(new LoggingMiddleware());
        return this;
    }

    public WebAppBuilder UseAuthenticationMiddleware()
    {
        _middleware.Use(new AuthenticationMiddleware());
        return this;
    }

    public WebAppBuilder UseFormParserMiddleware()
    {
        _middleware.Use(new FormParserMiddleware());
        return this;
    }

    public void Run()
    {
        var listener = new HttpListenerService(_launchUrl, _middleware);
        listener.RunAsync().GetAwaiter().GetResult();
    }
}