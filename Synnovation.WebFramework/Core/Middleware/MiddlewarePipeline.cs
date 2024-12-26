namespace Synnovation.WebFramework.Core.Middleware;

/// <summary>
/// Pipeline to process middleware components in sequence.
/// </summary>
public class MiddlewarePipeline
{
    private readonly List<MiddlewareBase> _middlewares = [];

    public MiddlewarePipeline Use(MiddlewareBase middleware)
    {
        _middlewares.Add(middleware);
        return this;
    }

    public async Task<HttpResponse> InvokeAsync(HttpRequest request, Func<HttpRequest, Task<HttpResponse>> finalHandler)
    {
        // Build chain: create a middleware chain by linking each middleware to the next
        var next = finalHandler;

        for (var i = _middlewares.Count - 1; i >= 0; i--)
        {
            var currentMiddleware = _middlewares[i];
            var previousNext = next;
            next = async req => await currentMiddleware.InvokeAsync(req, previousNext);
        }

        // Invoke the first middleware in the chain
        return await next(request);
    }
}