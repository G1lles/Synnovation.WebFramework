using Synnovation.WebFramework.Annotations;
using Synnovation.WebFramework.Core.Controllers;
using Synnovation.WebFramework.Core.Types;

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

    // GET /users/{id}?includeDetails=true
    [HttpGet("/users/{id}")]
    public IActionResult GetUserDetails(int id, [FromQuery] bool includeDetails)
    {
        // Simulating
        var user = new
        {
            Id = id,
            Name = "Test User",
            Age = 25,
            Email = "testuser@example.com",
            Address = "123 Demo Street"
        };

        ViewData["Title"] = "User Details";
        ViewData["Message"] = $"Details for User ID: {id}";

        ViewData["Id"] = user.Id;
        ViewData["Name"] = user.Name;
        ViewData["Age"] = user.Age;

        if (includeDetails)
        {
            ViewData["FullDetails"] = true;
            ViewData["DetailsMessage"] = "Full details are included.";
            ViewData["Email"] = user.Email;
            ViewData["Address"] = user.Address;
        }
        else
        {
            ViewData["DetailsMessage"] = "Basic details are shown.";
            ViewData["FullDetails"] = false;
        }

        return View("UserDetails");
    }
}