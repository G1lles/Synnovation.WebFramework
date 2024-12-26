using Synnovation.WebFramework.Builder;
using Synnovation.WebFramework.Core.Middleware.Implementations;

namespace Synnovation.WebFramework.Demo;

internal class Program
{    
    private static void Main(string[] args)
    {
        var builder = new WebAppBuilder("http://localhost:5000/");

        builder
            .AutoRegisterControllers(typeof(Program).Assembly)
            .UseStaticFiles()
            .UseLoggingMiddleware()
            .UseAuthenticationMiddleware()
            .UseFormParserMiddleware()
            .Run();
    }
}