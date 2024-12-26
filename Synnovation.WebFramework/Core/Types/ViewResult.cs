using Synnovation.WebFramework.Views;

namespace Synnovation.WebFramework.Core.Types;

/// <summary>
/// Renders an HTML view using the ViewEngine.
/// </summary>
public class ViewResult(string viewName, ViewData viewData) : IActionResult
{
    public HttpResponse ExecuteResult()
    {
        var html = ViewEngine.Render(viewName, viewData);
        return new HttpResponse(200, html)
        {
            ContentType = "text/html"
        };
    }
}