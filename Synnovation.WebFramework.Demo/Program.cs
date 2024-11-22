using Synnovation.WebFramework.Builder;
using Synnovation.WebFramework.Core.Middleware;
using Synnovation.WebFramework.Views;

namespace Synnovation.WebFramework.Demo;

internal class Program
{
    private static void Main(string[] args)
    {
        // Configure the custom views folder
        ViewEngine.Configure("Views");

        var builder = new WebAppBuilder(["http://localhost:5000/"]);

        builder
            .AutoRegisterControllers(typeof(Program).Assembly)
            .ConfigureMiddleware(middleware => { middleware.Use(new LoggingMiddleware(null)); })
            .Run();
    }
}