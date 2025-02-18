namespace Synnovation.WebFramework.Core.Middleware;

public abstract class MiddlewareBase
{
    public abstract Task<HttpResponse> InvokeAsync(HttpRequest request, Func<HttpRequest, Task<HttpResponse>> next);
}