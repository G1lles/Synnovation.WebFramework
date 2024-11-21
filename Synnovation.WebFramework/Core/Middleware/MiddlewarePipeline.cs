using System;
using System.Threading.Tasks;

namespace Synnovation.WebFramework.Core.Middleware;

/// <summary>
/// Pipeline to process middleware components in sequence.
/// </summary>
public class MiddlewarePipeline
{
    private MiddlewareBase? _first;

    public MiddlewarePipeline Use(MiddlewareBase middleware)
    {
        if (_first == null)
        {
            _first = middleware;
        }
        else
        {
            var current = _first;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = middleware;
        }
        return this;
    }

    public async Task<HttpResponse> InvokeAsync(HttpRequest request, Func<HttpRequest, Task<HttpResponse>> finalHandler)
    {
        if (_first != null)
        {
            return await _first.InvokeAsync(request, finalHandler);
        }
        else
        {
            // No middleware, directly invoke the final handler
            return await finalHandler(request);
        }
    }
}