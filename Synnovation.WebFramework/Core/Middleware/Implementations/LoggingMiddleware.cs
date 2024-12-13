namespace Synnovation.WebFramework.Core.Middleware.Implementations;

/// <summary>
/// Middleware for logging incoming requests and outgoing responses.
/// </summary>
public class LoggingMiddleware : MiddlewareBase
{
    public override async Task<HttpResponse> InvokeAsync(HttpRequest request, Func<HttpRequest, Task<HttpResponse>> next)
    {
        Console.WriteLine($"[{DateTime.Now}] Incoming request: {request.Method} {request.Path}");

        var response = await next(request);

        Console.WriteLine($"[{DateTime.Now}] Outgoing response: {response.StatusCode}");
        return response;
    }
}