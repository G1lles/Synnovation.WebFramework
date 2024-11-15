using Synnovation.WebFramework.Mvc.Routing;

namespace Synnovation.WebFramework.Mvc;

public class HttpPostAttribute : HttpMethodAttribute
{
    private static readonly List<string> SupportedMethods = ["POST"];

    public HttpPostAttribute() 
        : base(SupportedMethods)
    {
    }
}