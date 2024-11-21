using Synnovation.WebFramework.Views;

namespace Synnovation.WebFramework.Core;

/// <summary>
/// Base class for all MVC controllers.
/// </summary>
public abstract class ControllerBase
{
    protected ViewData ViewData { get; } = new();

    protected string Json(object data)
    {
        return System.Text.Json.JsonSerializer.Serialize(data);
    }

    protected string View(string viewName)
    {
        return ViewEngine.Render(viewName, ViewData);
    }

    protected string View(string viewName, object model)
    {
        ViewData.Model = model;
        return View(viewName);
    }
}