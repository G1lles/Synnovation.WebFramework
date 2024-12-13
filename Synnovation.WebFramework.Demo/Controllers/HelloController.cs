using Synnovation.WebFramework.Annotations;
using Synnovation.WebFramework.Core;

namespace Synnovation.WebFramework.Demo.Controllers;

public class HelloController : ControllerBase
{
    // Without [HttpGet] to demonstrate
    public string Index()
    {
        ViewData["Title"] = "Synnovation .NET";
        ViewData["Message"] = "This is a dynamically rendered view.";
        ViewData["ShowItems"] = true;
        ViewData["Items"] = new List<string>
        {
            "Feature 1: Routing",
            "Feature 2: Middleware",
            "Feature 3: Views",
            "Feature 4: DI Container (Scoped)"
        };

        return View("Hello");
    }

    [HttpPost("/post")]
    [HttpGet("/view")]
    public string View()
    {
        ViewData["Title"] = "Synnovation .NET";
        ViewData["Message"] = "This is a dynamically rendered view.";
        ViewData["ShowItems"] = true;
        ViewData["Items"] = new List<string> { "Feature 1: Routing", "Feature 2: Middleware", "Feature 3: Views" };

        return View("Hello");
    }

    [Authorize]
    public string Protected()
    {
        return View("Hello");
    }
}