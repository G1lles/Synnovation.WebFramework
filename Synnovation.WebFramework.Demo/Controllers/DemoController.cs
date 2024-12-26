using Synnovation.WebFramework.Annotations;
using Synnovation.WebFramework.Core;

namespace Synnovation.WebFramework.Demo.Controllers;

public class DemoController : MvcControllerBase
{
    // GET /Index (no [HttpGet] attribute -> defaults to GET "/Index")
    [HttpGet("/welcome")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Synnovation .NET";
        ViewData["Message"] = "This is a dynamically rendered view.";
        ViewData["ShowItems"] = true;
        ViewData["Items"] = new List<string>
        {
            "Feature 1: Routing",
            "Feature 2: Middleware Pipeline",
            "Feature 3: Controllers",
            "Feature 4: View Engine",
            "Feature 5: DI Parameter Binding"
        };

        return View("Welcome");
    }

    [HttpPost("/create-user")]
    public IActionResult CreateUser()
    {
        var name = Request?.Form.GetValueOrDefault("Name") ?? "(No Name Provided)";
        var ageString = Request?.Form.GetValueOrDefault("Age") ?? "0";

        int.TryParse(ageString, out var age);

        ViewData["Title"] = "User Created Successfully";
        ViewData["Message"] = "You have posted new user data!";
        ViewData["Name"] = name;
        ViewData["Age"] = age;

        return View("SubmitResult");
    }

    [Authorize]
    public IActionResult Protected()
    {
        ViewData["Title"] = "Protected Content";
        ViewData["Message"] = "Only authorized users can see this!";
        return View("Welcome");
    }
}