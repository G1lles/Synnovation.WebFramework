using Synnovation.WebFramework.Core;
using Synnovation.WebFramework.Routing;

namespace Synnovation.WebFramework.Demo.Controllers;

public class ProductController : ControllerBase
{
    [HttpGet("/product/{id}/{category}")]
    public string GetProduct(string id, string category)
    {
        // Simulate fetching product details
        ViewData["ProductId"] = id;
        ViewData["Category"] = category ?? "General";
        ViewData["Title"] = "Product Details";
        ViewData["Description"] = "This is a demo product description.";
        ViewData["Price"] = "$99.99";

        // Render the view
        return View("Product");
    }
}