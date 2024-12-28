using Synnovation.WebFramework.Builder;
using Synnovation.WebFramework.Demo.Middleware;

namespace Synnovation.WebFramework.Demo;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = new WebAppBuilder("http://localhost:5000/");

        builder
            .AutoRegisterControllers(typeof(Program).Assembly)
            .ConfigureMiddleware(pipeline => pipeline.Use(new TimingMiddleware())) // Custom middleware
            .UseStaticFiles()
            .UseLoggingMiddleware()
            .UseAuthenticationMiddleware()
            .UseFormParserMiddleware()
            .Run();
    }
}