using Synnovation.WebFramework.Mvc.Routing;

namespace Synnovation.WebFramework.Mvc;

public class HttpPutAttribute : HttpMethodAttribute
{
    private static readonly List<string> SupportedMethods = ["PUT"];

    public HttpPutAttribute() 
        : base(SupportedMethods)
    {
    }
}