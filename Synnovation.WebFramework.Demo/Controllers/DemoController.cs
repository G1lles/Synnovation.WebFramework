using Synnovation.WebFramework.Annotations;
using Synnovation.WebFramework.Core;

namespace Synnovation.WebFramework.Demo.Controllers;

public class DemoController : MvcControllerBase
{
    // GET /Index (no [HttpGet] attribute -> defaults to GET "/Index")
    public IActionResult Index()
    {
        ViewData["Title"] = "Synnovation .NET";
        ViewData["Message"] = "This is a dynamically rendered view (from Index).";
        ViewData["ShowItems"] = true;
        ViewData["Items"] = new List<string>
        {
            "Feature 1: Routing",
            "Feature 2: Middleware",
            "Feature 3: Views",
            "Feature 4: DI Container (Scoped)"
        };

        return View("index");
    }

    // GET /view
    [HttpGet("/view")]
    public IActionResult ViewExample()
    {
        ViewData["Title"] = "Synnovation .NET";
        ViewData["Message"] = "This is a dynamically rendered view (from ViewExample).";
        ViewData["ShowItems"] = true;
        ViewData["Items"] = new List<string>
        {
            "Feature 1: Routing",
            "Feature 2: Middleware",
            "Feature 3: Views"
        };

        return View("index");
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
        return View("index");
    }
}

// Optional, for demonstration if you want to parse JSON:
public record MyDataModel(string Name, int Age);