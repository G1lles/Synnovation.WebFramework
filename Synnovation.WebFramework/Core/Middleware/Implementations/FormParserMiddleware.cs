using System.Net;

namespace Synnovation.WebFramework.Core.Middleware.Implementations;

/// <summary>
/// Middleware that parses form-encoded data (application/x-www-form-urlencoded)
/// and populates HttpRequest.Form.
/// </summary>
public class FormParserMiddleware : MiddlewareBase
{
    public override async Task<HttpResponse> InvokeAsync(
        HttpRequest request,
        Func<HttpRequest, Task<HttpResponse>> next)
    {
        // Check if the request might contain form data.
        // For example, only parse if it's POST with a matching Content-Type.
        if (request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase)
            && request.Headers.TryGetValue("Content-Type", out var contentType)
            && contentType.Contains("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
        {
            // If the body is empty, we have nothing to parse.
            if (!string.IsNullOrWhiteSpace(request.Body))
            {
                // Example raw body: "Name=Alice&Age=30"
                var formParams = request.Body.Split('&', StringSplitOptions.RemoveEmptyEntries);

                foreach (var param in formParams)
                {
                    // Each param is "key=value"
                    var pair = param.Split('=', 2);
                    if (pair.Length == 2)
                    {
                        var key = pair[0];
                        var value = WebUtility.UrlDecode(pair[1]); // decode URL-encoded chars
                        request.Form[key] = value;
                    }
                }
            }
        }

        // Proceed to the next middleware (or final handler).
        return await next(request);
    }
}