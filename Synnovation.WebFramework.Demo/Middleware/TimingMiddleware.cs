using System.Diagnostics;
using Synnovation.WebFramework.Core;
using Synnovation.WebFramework.Core.Middleware;

namespace Synnovation.WebFramework.Demo.Middleware;

/**
 * Example to show how easy it is to implement custom Middleware
 */
public class TimingMiddleware : MiddlewareBase
{
    public override async Task<HttpResponse> InvokeAsync(HttpRequest request,
        Func<HttpRequest, Task<HttpResponse>> next)
    {
        var stopwatch = Stopwatch.StartNew();

        // Very important to call the next middleware or the chain stops
        var response = await next(request);

        stopwatch.Stop();

        // Add processing time to the response headers
        response.Headers["X-Processing-Time"] = $"{stopwatch.ElapsedMilliseconds} ms";
        Console.WriteLine($"[{DateTime.Now}] Processing time: {response.Headers["X-Processing-Time"]}");

        return await next(request);
    }
}