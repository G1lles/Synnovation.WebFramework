using Synnovation.WebFramework.Core.Middleware;
using Synnovation.WebFramework.Exceptions;
using Synnovation.WebFramework.Routing;

namespace Synnovation.WebFramework.Core;

/// <summary>
/// Main HTTP service responsible for processing requests.
/// </summary>
public class HttpListenerService
{
    private readonly CustomHttpListener _listener;
    private readonly MiddlewarePipeline _middleware;

    public HttpListenerService(string prefixes, MiddlewarePipeline middleware)
    {
        var port = int.Parse(prefixes.Split(':')[2].TrimEnd('/'));
        _listener = new CustomHttpListener(port);
        _middleware = middleware;
    }

    public async Task RunAsync()
    {
        _listener.Start();
        Console.WriteLine("Server started and listening for requests...");

        await _listener.ListenAsync(async request =>
        {
            HttpResponse response;

            try
            {
                // Middleware pipeline invocation
                response = await _middleware.InvokeAsync(
                    request,
                    context => Task.FromResult(RouteHandler.HandleRequest(context))
                );
            }
            catch (RouteNotFoundException ex)
            {
                // 404 for custom route not found
                response = new HttpResponse(404, ex.Message);
            }
            catch (ControllerNotFoundException ex)
            {
                response = new HttpResponse(500, ex.Message);
            }
            catch (ActionNotFoundException ex)
            {
                response = new HttpResponse(500, ex.Message);
            }
            catch (Exception ex)
            {
                response = new HttpResponse(500, $"Internal server error: {ex.Message}");
            }

            // Return the final response
            return response;
        });
    }

    public void Stop()
    {
        _listener.Stop();
    }
}