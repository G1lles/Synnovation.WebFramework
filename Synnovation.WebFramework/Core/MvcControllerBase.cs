using Synnovation.WebFramework.Views;

namespace Synnovation.WebFramework.Core;

/// <summary>
/// Base class for all MVC controllers.
/// </summary>
public abstract class MvcControllerBase
{
    public HttpRequest? Request { get; internal set; }
    protected ViewData ViewData { get; } = new();

    /// <summary>
    /// Renders a specific view (HTML) as a ViewResult.
    /// </summary>
    protected IActionResult View(string viewName)
    {
        return new ViewResult(viewName, ViewData);
    }

    protected IActionResult View(string viewName, object model)
    {
        ViewData.Model = model;
        return View(viewName);
    }
}