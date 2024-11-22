using Synnovation.WebFramework.Core;
using Synnovation.WebFramework.Routing;

namespace Synnovation.WebFramework.Demo.Controllers;

public class HelloController : ControllerBase
{
    [HttpGet("/test")]
    public string Get()
    {
        ViewData["Title"] = "Welcome to the Demo Application";
        ViewData["Message"] = "This is a dynamically rendered view using the custom framework.";
        ViewData["ShowItems"] = true;
        ViewData["Items"] = new List<string> { "Feature 1: Routing", "Feature 2: Middleware", "Feature 3: Views" };

        return View("Hello");
    }
}