using System.Text.Json;
using Synnovation.WebFramework.Views;

namespace Synnovation.WebFramework.Core;

/// <summary>
/// Base class for all MVC controllers.
/// </summary>
public abstract class ControllerBase
{
    /// <summary>
    /// Exposes the current HTTP request to the controller.
    /// </summary>
    public HttpRequest? Request { get; internal set; }

    protected ViewData ViewData { get; } = new();

    /// <summary>
    /// Helper method to serialize an object to JSON.
    /// </summary>
    protected static string Json(object data)
    {
        return JsonSerializer.Serialize(data);
    }

    /// <summary>
    /// Renders the specified view with the current <see cref="ViewData"/>.
    /// </summary>
    protected string View(string viewName)
    {
        return ViewEngine.Render(viewName, ViewData);
    }

    /// <summary>
    /// Renders the specified view, also setting a model in <see cref="ViewData"/>.
    /// </summary>
    protected string View(string viewName, object model)
    {
        ViewData.Model = model;
        return View(viewName);
    }

    /// <summary>
    /// Attempts to parse the incoming request body as JSON of type T.
    /// </summary>
    protected T? ParseJsonBody<T>()
    {
        if (Request?.Body == null || string.IsNullOrWhiteSpace(Request.Body))
            return default;

        return JsonSerializer.Deserialize<T>(Request.Body);
    }
}