using Synnovation.WebFramework.Routing;

namespace Synnovation.WebFramework.Core.Middleware.Implementations;

public class AuthenticationMiddleware : MiddlewareBase
{
    public override async Task<HttpResponse> InvokeAsync(HttpRequest request,
        Func<HttpRequest, Task<HttpResponse>> next)
    {
        // Check if the matched route requires authorization
        var matchedRoute = RouteTable.Instance.Routes.FirstOrDefault(r =>
            r.Path.Equals(request.Path, StringComparison.OrdinalIgnoreCase) &&
            r.HttpMethod.Equals(request.Method, StringComparison.OrdinalIgnoreCase));

        if (matchedRoute?.RequiresAuthorization == true)
        {
            // Pre-processing: Check for authentication token in headers
            if (!request.Headers.TryGetValue("Authorization", out var value) || string.IsNullOrEmpty(value))
            {
                Console.WriteLine("Authentication Middleware: Unauthorized request.");
                return new HttpResponse(401, "Unauthorized");
            }

            Console.WriteLine("Authentication Middleware: Authorized request.");
        }

        // Call the next middleware in the pipeline
        var response = await next(request);

        // Post-processing: Optionally modify the response
        response.Headers["X-Auth-Processed"] = "True";
        return response;
    }
}