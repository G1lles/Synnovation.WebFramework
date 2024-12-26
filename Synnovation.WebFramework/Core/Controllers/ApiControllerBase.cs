using System.Text.Json;
using Synnovation.WebFramework.Core.Types;

namespace Synnovation.WebFramework.Core.Controllers;

public abstract class ApiControllerBase
{
    public HttpRequest? Request { get; internal set; }

    /// <summary>
    /// Returns 200 OK with JSON
    /// </summary>
    protected IActionResult Ok(object? data = null)
    {
        return new JsonResult(data, 200);
    }

    /// <summary>
    /// Returns 201 Created with JSON
    /// </summary>
    protected IActionResult Created(object? data = null)
    {
        return new JsonResult(data, 201);
    }

    /// <summary>
    /// Returns 400 Bad Request with JSON
    /// </summary>
    protected IActionResult BadRequest(object? error = null)
    {
        return new JsonResult(error, 400);
    }

    /// <summary>
    /// Returns 404 Not Found with JSON
    /// </summary>
    protected IActionResult NotFound(object? error = null)
    {
        return new JsonResult(error, 404);
    }

    /// <summary>
    /// Return any arbitrary status code with JSON
    /// </summary>
    protected IActionResult StatusCode(int statusCode, object? data = null)
    {
        return new JsonResult(data, statusCode);
    }

    /// <summary>
    /// Parse the request body as JSON of type T automatically
    /// (still optional—your framework can do param binding).
    /// </summary>
    protected T? ParseJsonBody<T>()
    {
        if (Request?.Body == null || string.IsNullOrWhiteSpace(Request.Body))
            return default;
        return JsonSerializer.Deserialize<T>(Request.Body);
    }
}
