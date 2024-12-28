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

    /// <summary>
    /// Initializes a new instance of the <see cref="WebAppBuilder"/> class.
    /// </summary>
    /// <param name="launchUrl">The URL the application should listen to.</param>
    /// <exception cref="ArgumentException">Thrown when the provided URL is null or empty.</exception>
    public WebAppBuilder(string launchUrl)
    {
        if (string.IsNullOrEmpty(launchUrl))
            throw new ArgumentException("You must specify at least one prefix for the application.");
        _launchUrl = launchUrl;
    }

    /// <summary>
    /// Builds and runs the application.
    /// </summary>
    /// <remarks>
    /// This method initializes the HTTP listener service with the configured middleware and begins listening for incoming requests.
    /// </remarks>
    public void Run()
    {
        var listener = new HttpListenerService(_launchUrl, _middleware);

        // Waits for the completion of the given task
        listener.RunAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Automatically registers all controllers in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan for controllers.</param>
    /// <returns>The current <see cref="WebAppBuilder"/> instance for method chaining.</returns>
    public WebAppBuilder AutoRegisterControllers(Assembly assembly)
    {
        RouteScanner.RegisterRoutesFromAssembly(RouteTable.Instance, assembly);
        return this;
    }

    /// <summary>
    /// Allows manual configuration of the middleware pipeline.
    /// </summary>
    /// <param name="configureMiddleware">
    /// An action that receives the <see cref="MiddlewarePipeline"/> to allow middleware customization.
    /// </param>
    /// Example usage: .ConfigureMiddleware(pipeline => { pipeline.Use(new Middleware()); }
    /// <returns>The current <see cref="WebAppBuilder"/> instance for method chaining.</returns>
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
}