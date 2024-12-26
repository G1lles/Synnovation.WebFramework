namespace Synnovation.WebFramework.Core;

/// <summary>
/// Returns a given status code with an optional message or payload.
/// </summary>
public class StatusCodeResult(int statusCode, string body = "", string contentType = "text/plain")
    : IActionResult
{
    public HttpResponse ExecuteResult()
    {
        return new HttpResponse(statusCode, body)
        {
            ContentType = contentType
        };
    }
}