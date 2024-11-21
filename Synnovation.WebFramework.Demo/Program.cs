using Synnovation.WebFramework.Builder;
using Synnovation.WebFramework.Core.Middleware;
using Synnovation.WebFramework.Demo.Controllers;
using Synnovation.WebFramework.Views;

namespace Synnovation.WebFramework.Demo;

class Program
{
    static void Main(string[] args)
    {
        // Configure the custom views folder
        ViewEngine.Configure("Views");

        var builder = new WebAppBuilder(new[] { "http://localhost:5000/" });

        builder
            .ConfigureRoutes(routes =>
            {
                routes.AddRoute("/", "GET", typeof(HelloController), nameof(HelloController.Get));
                routes.AddRoute("/product/{id}", "GET", typeof(ProductController), nameof(ProductController.GetProduct));
            })
            .Run();
    }
}