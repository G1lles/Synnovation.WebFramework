namespace Synnovation.WebFramework.Core.Middleware;

public abstract class MiddlewareBase(MiddlewareBase? next)
{ 

   public MiddlewareBase? Next { get; set; } = next;

   public abstract Task<HttpResponse> InvokeAsync(HttpRequest request, Func<HttpRequest, Task<HttpResponse>> next);
}
