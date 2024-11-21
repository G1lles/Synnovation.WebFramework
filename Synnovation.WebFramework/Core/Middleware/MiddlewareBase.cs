namespace Synnovation.WebFramework.Core.Middleware;

public abstract class MiddlewareBase
{ 

   public MiddlewareBase? Next { get; set; }

    protected MiddlewareBase(MiddlewareBase? next)
    {
        Next = next;
    }

    public abstract Task<HttpResponse> InvokeAsync(HttpRequest request, Func<HttpRequest, Task<HttpResponse>> next);
}
