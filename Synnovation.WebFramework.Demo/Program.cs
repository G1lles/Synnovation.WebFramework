using Synnovation.WebFramework.Builder;
using Synnovation.WebFramework.Core.Middleware;

namespace Synnovation.WebFramework.Demo;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = new WebAppBuilder(["http://localhost:5000/"]);
        
        builder
            .AutoRegisterControllers(typeof(Program).Assembly)
            .UseStaticFiles()
            .ConfigureMiddleware(middleware =>
            {
                middleware.Use(new LoggingMiddleware(null));
            })
            .Run();
    }
}